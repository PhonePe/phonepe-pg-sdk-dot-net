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
public class UpiCollectPayViaVpaRequestBuilderTests
{
    [Test]
    public void Build_WithValidInputs_ShouldConstructPgPaymentRequest()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf2("vpa info")
            .Build();
            
        var constraints = new List<InstrumentConstraint>();
        
        var request = new UpiCollectPayViaVpaRequestBuilder()
            .SetMerchantOrderId("order456")
            .SetAmount(1500)
            .SetMetaInfo(metaInfo)
            .SetConstraints(constraints)
            .SetVpa("user@upi")
            .SetMessage("Pay using VPA")
            .SetExpireAfter(600)
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("order456"));
        Assert.That(request.Amount, Is.EqualTo(1500));
        Assert.That(request.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(request.Constraints, Is.EqualTo(constraints));
        Assert.That(request.ExpireAfter, Is.EqualTo(600));

        var paymentFlow = request.PaymentFlow;
        Assert.That(paymentFlow, Is.Not.Null);
        Assert.That(paymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.UPI_COLLECT));

        var instrument = paymentFlow.PaymentMode as CollectPaymentV2Instrument;
        Assert.That(instrument!.Details, Is.TypeOf<VpaCollectPaymentDetails>());
        Assert.That(instrument.Message, Is.EqualTo("Pay using VPA"));

        var vpaDetails = (VpaCollectPaymentDetails)instrument.Details;
        Assert.That(vpaDetails.Vpa, Is.EqualTo("user@upi"));
    }

    [Test]
    public void Build_VpaSerialisesToWireFormat_VpaNotDropped()
    {
        var request = new UpiCollectPayViaVpaRequestBuilder()
            .SetMerchantOrderId("order789")
            .SetAmount(500)
            .SetVpa("merchant@upi")
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions.IndentedWithPaymentConverters);
        using var doc = JsonDocument.Parse(json);

        // Navigate: paymentFlow -> paymentMode -> details -> vpa
        var details = doc.RootElement
            .GetProperty("paymentFlow")
            .GetProperty("paymentMode")
            .GetProperty("details");

        Assert.That(details.TryGetProperty("vpa", out var vpaProp), Is.True,
            "vpa must be present in the serialized wire payload");
        Assert.That(vpaProp.GetString(), Is.EqualTo("merchant@upi"));

        Assert.That(details.TryGetProperty("type", out var typeProp), Is.True,
            "type must be present as a string, not an integer");
        Assert.That(typeProp.GetString(), Is.EqualTo("VPA"));
    }
}
