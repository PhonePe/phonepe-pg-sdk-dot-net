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
using pg_sdk_dotnet.Common.Constants;
using pg_sdk_dotnet.Payments.v2.CustomCheckout;

namespace pg_sdk_dotnet.tests;

[NonParallelizable]
[TestFixture]
public class PciRoutingTests : BaseSetupWithOAuth
{
    private CustomCheckoutClient customCheckoutClient;

    [SetUp]
    public void Setup()
    {
        var env = Env.TESTING;

        customCheckoutClient = CustomCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            env
        );
    }

    [Test, Order(1)]
    public async Task TestCardInstrumentPayRoutesToPciHostUrl()
    {
        string url = CustomCheckoutConstants.PAY_API;

        OAuthSetup();

        var cardPayRequest = new CardPayRequestBuilder()
            .SetMerchantOrderId("card_pci_order_001")
            .SetAmount(500)
            .SetEncryptionKeyId(123)
            .SetEncryptedCardNumber("encCard123")
            .SetEncryptedCvv("encCvv123")
            .SetExpiryMonth("12")
            .SetExpiryYear("2026")
            .Build();

        var customCheckoutResponse = new CustomCheckoutPayResponse
        {
            OrderId = "orderId",
            State = "PENDING",
            ExpireAt = 300,
            RedirectUrl = "https://google.com"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(url, headers, cardPayRequest, 200, new Dictionary<string, string>(), customCheckoutResponse);

        var result = await customCheckoutClient.Pay(cardPayRequest);
        Assert.That(result.OrderId, Is.EqualTo("orderId"));
        Assert.That(result.State, Is.EqualTo("PENDING"));
        Assert.That(result.RedirectUrl, Is.EqualTo("https://google.com"));
    }

    [Test, Order(2)]
    public async Task TestTokenInstrumentPayRoutesToPciHostUrl()
    {
        string url = CustomCheckoutConstants.PAY_API;

        OAuthSetup();

        var tokenPayRequest = new TokenPayRequestBuilder()
            .SetMerchantOrderId("token_pci_order_001")
            .SetAmount(750)
            .SetEncryptionKeyId(1)
            .SetEncryptedToken("encToken123")
            .SetEncryptedCvv("encCvv123")
            .SetCryptogram("cryptogram123")
            .SetPanSuffix("1234")
            .SetExpiryMonth("08")
            .SetExpiryYear("2027")
            .Build();

        var customCheckoutResponse = new CustomCheckoutPayResponse
        {
            OrderId = "tokenOrderId",
            State = "PENDING",
            ExpireAt = 300,
            RedirectUrl = "https://google.com"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(url, headers, tokenPayRequest, 200, new Dictionary<string, string>(), customCheckoutResponse);

        var result = await customCheckoutClient.Pay(tokenPayRequest);
        Assert.That(result.OrderId, Is.EqualTo("tokenOrderId"));
        Assert.That(result.State, Is.EqualTo("PENDING"));
        Assert.That(result.RedirectUrl, Is.EqualTo("https://google.com"));
    }

    [Test, Order(3)]
    public async Task TestUpiCollectPayUsesDefaultHostUrl()
    {
        string url = CustomCheckoutConstants.PAY_API;

        OAuthSetup();

        var upiCollectRequest = PgPaymentRequest.UpiCollectPayViaVpaRequestBuilder()
            .SetMerchantOrderId("upi_collect_order_001")
            .SetAmount(200)
            .SetVpa("test@upi")
            .Build();

        var customCheckoutResponse = new CustomCheckoutPayResponse
        {
            OrderId = "upiOrderId",
            State = "PENDING",
            ExpireAt = 300,
            RedirectUrl = "https://google.com"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(url, headers, upiCollectRequest, 200, new Dictionary<string, string>(), customCheckoutResponse);

        var result = await customCheckoutClient.Pay(upiCollectRequest);
        Assert.That(result.OrderId, Is.EqualTo("upiOrderId"));
        Assert.That(result.State, Is.EqualTo("PENDING"));
        Assert.That(result.RedirectUrl, Is.EqualTo("https://google.com"));
    }
}
