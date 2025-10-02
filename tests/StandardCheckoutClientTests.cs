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

using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[TestFixture]
[NonParallelizable]
public class StandardCheckoutClientTests : BaseSetupWithOAuth
{
    private StandardCheckoutClient standardCheckoutClient;
    private Dictionary<string, string> FormData;


    [SetUp]
    public void Setup()
    {
       var env = Env.TESTING;

        standardCheckoutClient = StandardCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            env
        );

        FormData = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "client_version", ClientVersion.ToString() },
            { "grant_type", "client_credentials" }
        };
    }

    [Test, Order(3)]
    public async Task MultipleClientsSingleAuthCall()
    {

        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("merchantOrderId")
            .SetAmount(100)
            .Build();

        var payUrl = "/checkout/v2/pay";
        var tokenUrl = "/v1/oauth/token";

        var mockPayResponse = new
        {
            orderId = "orderId",
            state = "PENDING",
            expireAt = 238462442,
            redirectUrl = "redirectUrl"
        };

        long issuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiresAt = issuedAt + 3600;

        OAuthResponse oAuthResponse = new OAuthResponse
        {
            AccessToken = "access_token",
            EncryptedAccessToken = "encryptedAccessToken",
            RefreshToken = "refreshToken",
            ExpiresIn = 2432,
            IssuedAt = issuedAt,
            ExpiresAt = expiresAt,
            SessionExpiresAt = 234543534,
            TokenType = "O-Bearer"
        };

        Dictionary<string, string> authHeaders = GetAuthHeaders();

        AddStubForFormDataPostRequest(tokenUrl, authHeaders, FormData, 200, new Dictionary<string, string>(), oAuthResponse);

        var payHeaders = GetHeadersForPostReq();

        AddStubForPostRequest(
            payUrl,
            payHeaders,
            request,
            200,
            new Dictionary<string, string>(),
            mockPayResponse
        );

        var client1 = standardCheckoutClient;
        var client2 = standardCheckoutClient;
        var client3 = standardCheckoutClient;
        var client4 = standardCheckoutClient;

        await client1.Pay(request);
        await client2.Pay(request);
        await client3.Pay(request);
        await client4.Pay(request);

        var oauthHits = wireMockServer.LogEntries.Count(entry => entry.RequestMessage.Path == tokenUrl && entry.RequestMessage.Method == "POST");
        var payHits = wireMockServer.LogEntries.Count(entry => entry.RequestMessage.Path == payUrl && entry.RequestMessage.Method == "POST");

        Assert.That(oauthHits, Is.EqualTo(1), "Only one OAuth token request should have occurred");
        Assert.That(payHits, Is.EqualTo(4), "Four pay requests should have been made");
    }
}
