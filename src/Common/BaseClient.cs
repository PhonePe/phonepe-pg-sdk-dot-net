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

namespace pg_sdk_dotnet.Common;

public abstract class BaseClient
{
    private readonly CredentialConfig _merchantConfig;
    private readonly TokenService _tokenService;
    private readonly ILogger<BaseClient> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly EnvConfig _envConfig;
    private readonly HttpClient _httpClient;

    protected BaseClient(
    string clientId,
    string clientSecret,
    int clientVersion,
    Env env,
    ILoggerFactory loggerFactory)
    {
        this._loggerFactory = loggerFactory;

        this._logger = loggerFactory.CreateLogger<BaseClient>();

        this._envConfig = EnvConfig.GetBaseUrls(env); ;

        this._httpClient = new();

        this._merchantConfig = new CredentialConfig(clientId, clientSecret, clientVersion);

        this._tokenService = new TokenService(this._merchantConfig, this._loggerFactory, this._envConfig);

    }

    /*
    * Executes an authenticated API request, refreshing the token if necessary.
    */
    protected Task<T> RequestViaAuthRefreshAsync<T, R>(
        HttpMethodType method,
        string endpoint,
        Dictionary<string, string> headers,
        string encodingType,
        R? requestData = default,
        Dictionary<string, string>? queryParams = null)
    {
        return RequestViaAuthRefreshAsync<T, R>(
            method,
            endpoint,
            headers,
            encodingType,
            this._envConfig.PgHostUrl,
            requestData,
            queryParams
        );
    }

    /*
    * Executes an authenticated API request with an explicit host URL, refreshing the token if necessary.
    */
    protected async Task<T> RequestViaAuthRefreshAsync<T, R>(
        HttpMethodType method,
        string endpoint,
        Dictionary<string, string> headers,
        string encodingType,
        string hostUrl,
        R? requestData = default,
        Dictionary<string, string>? queryParams = null)
    {
        var httpHeaders = await AddAuthHeaderAsync(headers);

        var httpCommandLogger = this._loggerFactory.CreateLogger<HttpCommand<T, R>>();

        try
        {
            var httpCommand = new HttpCommand<T, R>(
                this._httpClient,
                hostUrl,
                endpoint,
                httpHeaders,
                requestData ?? default!,
                encodingType,
                method,
                queryParams ?? [],
                httpCommandLogger
            );

            return await httpCommand.ExecuteAsync();
        }
        catch (UnauthorizedAccess)
        {
            await this._tokenService.ForceRefreshTokenAsync(this._httpClient);

            httpHeaders = await AddAuthHeaderAsync(headers);

            var retryHttpCommand = new HttpCommand<T, R>(
                this._httpClient,
                hostUrl,
                endpoint,
                httpHeaders,
                requestData ?? default!,
                encodingType,
                method,
                queryParams ?? [],
                httpCommandLogger
            );

            return await retryHttpCommand.ExecuteAsync();
        }
    }

    /*
    * Adds the OAuth Authorization header to the request.
    */
    private async Task<Dictionary<string, string>> AddAuthHeaderAsync(Dictionary<string, string> headers)
    {
        var updatedHeaders = new Dictionary<string, string>(headers);
        string authToken = await this._tokenService.GetOAuthTokenAsync(this._httpClient);
        updatedHeaders.Add(Headers.OAUTH_AUTHORIZATION, authToken);
        return updatedHeaders;
    }

    /*
    * Getters for BaseClient properties
    */
    public CredentialConfig MerchantConfig => this._merchantConfig;
    protected string PciPgHostUrl => this._envConfig.PciPgHostUrl;
}
