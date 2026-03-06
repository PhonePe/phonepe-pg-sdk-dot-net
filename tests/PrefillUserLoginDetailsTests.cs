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

using System.Text.Json;
using NUnit.Framework;
using pg_sdk_dotnet.Payments.v2.StandardCheckout;

namespace pg_sdk_dotnet.tests;

[TestFixture]
[NonParallelizable]
public class PrefillUserLoginDetailsTests : BaseSetupWithOAuth
{
    private StandardCheckoutClient standardCheckoutClient;

    private static readonly JsonSerializerOptions CamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private static readonly StandardCheckoutPayResponse MockPayResponse = new()
    {
        OrderId = "OMO2403071446458436434329",
        State = "PENDING",
        ExpireAt = 1709803425841,
        RedirectUrl = "https://pay.phonepe.com/redirect"
    };

    [SetUp]
    public void Setup()
    {
        standardCheckoutClient = StandardCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            Env.TESTING
        );
    }

    // ------------------------------------------------------------------ //
    // Model-level tests                                                    //
    // ------------------------------------------------------------------ //

    [Test]
    public void TestPrefillUserLoginDetailsWithPhoneNumber()
    {
        var prefill = new PrefillUserLoginDetails("9999999999");

        Assert.That(prefill.PhoneNumber, Is.EqualTo("9999999999"));
    }

    [Test]
    public void TestPrefillUserLoginDetailsDefaultPhoneNumberIsNull()
    {
        var prefill = new PrefillUserLoginDetails();

        Assert.That(prefill.PhoneNumber, Is.Null);
    }

    [Test]
    public void TestPrefillUserLoginDetailsBuildWithBuilder()
    {
        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        Assert.That(prefill.PhoneNumber, Is.EqualTo("9999999999"));
    }

    // ------------------------------------------------------------------ //
    // StandardCheckoutPayRequest builder integration tests                //
    // ------------------------------------------------------------------ //

    [Test]
    public void TestBuildRequestWithPrefillUserLoginDetails()
    {
        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER001")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetPrefillUserLoginDetails(prefill)
            .Build();

        Assert.That(request.PrefillUserLoginDetails, Is.Not.Null);
        Assert.That(request.PrefillUserLoginDetails!.PhoneNumber, Is.EqualTo("9999999999"));
    }

    [Test]
    public void TestBuildRequestWithoutPrefillUserLoginDetails()
    {
        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER002")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .Build();

        Assert.That(request.PrefillUserLoginDetails, Is.Null);
    }

    [Test]
    public void TestBuildRequestWithNullPrefillUserLoginDetails()
    {
        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER003")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetPrefillUserLoginDetails(null)
            .Build();

        Assert.That(request.PrefillUserLoginDetails, Is.Null);
    }

    // ------------------------------------------------------------------ //
    // Serialization tests                                                  //
    // ------------------------------------------------------------------ //

    [Test]
    public void TestPrefillUserLoginDetailsSerializesToCamelCase()
    {
        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        var json = JsonSerializer.Serialize(prefill, CamelCaseOptions);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.TryGetProperty("phoneNumber", out var phoneNumberProp), Is.True,
            "phoneNumber (camelCase) key should be present");
        Assert.That(phoneNumberProp.GetString(), Is.EqualTo("9999999999"));
        Assert.That(root.TryGetProperty("PhoneNumber", out _), Is.False,
            "PascalCase PhoneNumber key should not be present");
    }

    [Test]
    public void TestRequestWithPrefillSerializedCorrectly()
    {
        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER004")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetPrefillUserLoginDetails(prefill)
            .Build();

        var json = JsonSerializer.Serialize(request, CamelCaseOptions);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.TryGetProperty("prefillUserLoginDetails", out var prefillProp), Is.True,
            "prefillUserLoginDetails key should be present");
        Assert.That(prefillProp.TryGetProperty("phoneNumber", out var phoneProp), Is.True);
        Assert.That(phoneProp.GetString(), Is.EqualTo("9999999999"));
    }

    [Test]
    public void TestRequestWithoutPrefillOmitsField()
    {
        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER005")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .Build();

        var json = JsonSerializer.Serialize(request, CamelCaseOptions);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.TryGetProperty("prefillUserLoginDetails", out _), Is.False,
            "prefillUserLoginDetails key should be absent when not set");
    }

    // ------------------------------------------------------------------ //
    // HTTP integration tests (pay API via WireMock)                       //
    // ------------------------------------------------------------------ //

    [Test, Order(1)]
    public async Task TestPayWithPrefillUserLoginDetails()
    {
        string url = "/checkout/v2/pay";

        OAuthSetup();

        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        var payRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER006")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetPrefillUserLoginDetails(prefill)
            .Build();

        var headers = GetHeadersForPostReq();
        AddStubForPostRequest(url, headers, payRequest, 200, new Dictionary<string, string>(), MockPayResponse);

        var result = await standardCheckoutClient.Pay(payRequest);

        Assert.That(result.OrderId, Is.EqualTo(MockPayResponse.OrderId));
        Assert.That(result.State, Is.EqualTo(MockPayResponse.State));
    }

    [Test, Order(2)]
    public async Task TestPayWithoutPrefillUserLoginDetails()
    {
        string url = "/checkout/v2/pay";

        OAuthSetup();

        var payRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER007")
            .SetAmount(1000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .Build();

        var headers = GetHeadersForPostReq();
        AddStubForPostRequest(url, headers, payRequest, 200, new Dictionary<string, string>(), MockPayResponse);

        var result = await standardCheckoutClient.Pay(payRequest);

        Assert.That(result.OrderId, Is.EqualTo(MockPayResponse.OrderId));
        Assert.That(result.State, Is.EqualTo(MockPayResponse.State));
    }

    [Test, Order(3)]
    public async Task TestPayWithPrefillAndOtherFields()
    {
        string url = "/checkout/v2/pay";

        OAuthSetup();

        var metaInfo = MetaInfo.Builder()
            .SetUdf1("udf1")
            .Build();

        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("8888888888")
            .Build();

        var payRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER008")
            .SetAmount(2000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetMessage("Test payment")
            .SetMetaInfo(metaInfo)
            .SetExpireAfter(3600)
            .SetPrefillUserLoginDetails(prefill)
            .Build();

        Assert.That(payRequest.PrefillUserLoginDetails, Is.Not.Null);
        Assert.That(payRequest.PrefillUserLoginDetails!.PhoneNumber, Is.EqualTo("8888888888"));
        Assert.That(payRequest.MetaInfo, Is.Not.Null);

        var headers = GetHeadersForPostReq();
        AddStubForPostRequest(url, headers, payRequest, 200, new Dictionary<string, string>(), MockPayResponse);

        var result = await standardCheckoutClient.Pay(payRequest);

        Assert.That(result.OrderId, Is.EqualTo(MockPayResponse.OrderId));
    }

    [Test]
    public async Task TestPayWithPrefillAndDisablePaymentRetry()
    {
        string url = StandardCheckoutConstants.PAY_API;

        OAuthSetup();

        var prefill = PrefillUserLoginDetails.Builder()
            .SetPhoneNumber("9999999999")
            .Build();

        var payRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("ORDER009")
            .SetAmount(3000)
            .SetRedirectUrl("https://merchant.com/redirect")
            .SetPrefillUserLoginDetails(prefill)
            .SetDisablePaymentRetry(true)
            .Build();

        Assert.That(payRequest.PrefillUserLoginDetails, Is.Not.Null);
        Assert.That(payRequest.PrefillUserLoginDetails!.PhoneNumber, Is.EqualTo("9999999999"));
        Assert.That(payRequest.DisablePaymentRetry, Is.True);

        var json = System.Text.Json.JsonSerializer.Serialize(payRequest);
        Assert.That(json, Does.Contain("DisablePaymentRetry"));
        Assert.That(json, Does.Contain("PrefillUserLoginDetails"));

        var headers = GetHeadersForPostReq();
        AddStubForPostRequest(url, headers, payRequest, 200, new Dictionary<string, string>(), MockPayResponse);

        var result = await standardCheckoutClient.Pay(payRequest);

        Assert.That(result.OrderId, Is.EqualTo(MockPayResponse.OrderId));
    }
}
