/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using Microsoft.Extensions.Logging;

namespace pg_sdk_dotnet.Common.TokenHandler;

/*
 * Handles fetching and caching OAuth tokens.
 * Uses dependency injection -> Requires merchant to pass EnvConfig.
 */
public class TokenService
{
    private readonly CredentialConfig _credentialConfig;
    private readonly ILogger<TokenService> _logger;
    private readonly ILogger<HttpCommand<OAuthResponse, Dictionary<string, string>>> _httpCommandLogger;
    private readonly EnvConfig _envConfig;
    private OAuthResponse? _cachedToken;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public TokenService(CredentialConfig credentialConfig, ILoggerFactory loggerFactory, EnvConfig envConfig)
    {
        this._credentialConfig = credentialConfig ?? throw new ArgumentNullException(nameof(credentialConfig));
        this._logger = loggerFactory.CreateLogger<TokenService>();
        this._httpCommandLogger = loggerFactory.CreateLogger<HttpCommand<OAuthResponse, Dictionary<string, string>>>();
        this._envConfig = envConfig ?? throw new ArgumentNullException(nameof(envConfig));
    }

    /*
     * Prepares headers for the request.
     */
    private static Dictionary<string, string> PrepareRequestHeaders()
    {
        return new Dictionary<string, string>
        {
            { Headers.CONTENT_TYPE, Headers.APPLICATION_FORM_URLENCODED },
            { Headers.ACCEPT, Headers.APPLICATION_JSON }
        };
    }

    /*
     * Converts OAuthRequest to form-urlencoded string.
     */
    private static Dictionary<string, string> PrepareFormBody(OAuthRequest request)
    {
        return new Dictionary<string, string>
            {
                { TokenConstants.OAUTH_CLIENT_ID, request.ClientId },
                { TokenConstants.OAUTH_CLIENT_SECRET, request.ClientSecret },
                { TokenConstants.OAUTH_CLIENT_VERSION, request.ClientVersion.ToString() },
                { TokenConstants.OAUTH_GRANT_TYPE_KEY, request.GrantType }
            };

    }

    /*
     * Retrieves the OAuth token, using a cached token if valid.
     */
    public async Task<string> GetOAuthTokenAsync(HttpClient httpClient)
    {
        if (IsCachedTokenValid())
        {
            return FormatCachedToken();
        }

        await FetchTokenFromPhonePe(httpClient);
        return FormatCachedToken();
    }

    /*
     * Forcefully refreshes the OAuth token.
     */
    public async Task ForceRefreshTokenAsync(HttpClient httpClient)
    {
        await FetchTokenFromPhonePe(httpClient, forceRefresh: true);
    }

    private async Task FetchTokenFromPhonePe(HttpClient httpClient, bool forceRefresh = false)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!forceRefresh && IsCachedTokenValid())
            {
                return;
            }

            var requestData = new OAuthRequest
            {
                ClientId = this._credentialConfig.ClientId,
                ClientSecret = this._credentialConfig.ClientSecret,
                ClientVersion = this._credentialConfig.ClientVersion,
                GrantType = TokenConstants.OAUTH_GRANT_TYPE
            };

            var headers = PrepareRequestHeaders();
            var formData = PrepareFormBody(requestData);

            var httpCommand = new HttpCommand<OAuthResponse, Dictionary<string, string>>(
                httpClient,
                this._envConfig.OAuthHostUrl,
                TokenConstants.OAUTH_GET_TOKEN,
                headers,
                formData,
                Headers.APPLICATION_FORM_URLENCODED,
                HttpMethodType.POST,
                [],
                this._httpCommandLogger
            );

            _cachedToken = await httpCommand.ExecuteAsync();
        }
        catch (System.Exception ex)
        {
            if (_cachedToken == null)
            {
                this._logger.LogError(ex, "Failed to fetch OAuth token from PhonePe.");
                throw;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private bool IsCachedTokenValid()
    {
        if (_cachedToken == null)
        {
            return false;
        }

        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var reloadTime = _cachedToken.IssuedAt + (_cachedToken.ExpiresAt - _cachedToken.IssuedAt) / 2;

        return currentTime < reloadTime;
    }

    private string FormatCachedToken()
    {
        return $"{_cachedToken?.TokenType} {_cachedToken?.AccessToken}";
    }

}
