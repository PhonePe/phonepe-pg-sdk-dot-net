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
using pg_sdk_dotnet.Common.Utils;

namespace pg_sdk_dotnet.tests;

[TestFixture]
public class UpiCollectPayViaPhoneNumberRequestTests
{
    [Test]
    public void Build_WithValidInputs_ShouldConstructPgPaymentRequest()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf1("info")
            .Build();
        
        var constraints = new List<InstrumentConstraint>();
        
        var request = new UpiCollectPayViaPhoneNumberRequestBuilder()
            .SetMerchantOrderId("order123")
            .SetAmount(999)
            .SetMetaInfo(metaInfo)
            .SetConstraints(constraints)
            .SetPhoneNumber("9876543210")
            .SetMessage("Pay using phone number")
            .SetExpireAfter(1800)
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("order123"));
        Assert.That(request.Amount, Is.EqualTo(999));
        Assert.That(request.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(request.Constraints, Is.EqualTo(constraints));
        Assert.That(request.ExpireAfter, Is.EqualTo(1800));

        var paymentFlow = request.PaymentFlow;
        Assert.That(paymentFlow, Is.Not.Null);
        Assert.That(paymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.UPI_COLLECT));

        var instrument = (CollectPaymentV2Instrument)paymentFlow.PaymentMode;
        Assert.That(instrument.Details, Is.TypeOf<PhoneNumberCollectPaymentDetails>());
        Assert.That(instrument.Message, Is.EqualTo("Pay using phone number"));

        var phoneDetails = (PhoneNumberCollectPaymentDetails)instrument.Details;
        Assert.That(phoneDetails.PhoneNumber, Is.EqualTo("9876543210"));
    }

    [Test]
    public void Build_PhoneNumberSerialisesToWireFormat_PhoneNumberNotDropped()
    {
        var request = new UpiCollectPayViaPhoneNumberRequestBuilder()
            .SetMerchantOrderId("order456")
            .SetAmount(500)
            .SetPhoneNumber("9876543210")
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions.IndentedWithPaymentConverters);
        using var doc = JsonDocument.Parse(json);

        // Navigate: paymentFlow -> paymentMode -> details -> phoneNumber
        var details = doc.RootElement
            .GetProperty("paymentFlow")
            .GetProperty("paymentMode")
            .GetProperty("details");

        Assert.That(details.TryGetProperty("phoneNumber", out var phoneProp), Is.True,
            "phoneNumber must be present in the serialized wire payload");
        Assert.That(phoneProp.GetString(), Is.EqualTo("9876543210"));

        Assert.That(details.TryGetProperty("type", out var typeProp), Is.True,
            "type must be present as a string, not an integer");
        Assert.That(typeProp.GetString(), Is.EqualTo("PHONE_NUMBER"));
    }
}
