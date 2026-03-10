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

using System.Text.Json.Serialization;

namespace pg_sdk_dotnet.Common.Models.Request;

/**
 * Creates a request based on different builder for different instruments.
 * 1. UpiIntentPayRequestBuilder -> Upi Intent
 * 2. UpiCollectPayViaVpaRequestBuilder -> Upi Collect
 * 3. UpiQrRequestBuilder -> Upi QR
 * 4. NetBankingPayRequestBuilder -> Netbanking
 * 5. TokenPayRequestBuilder -> Token
 * 6. CardPayRequestBuilder -> Card
 */

 public class PgPaymentRequest(
     string merchantOrderId,
     long amount,
     PaymentFlow paymentFlow,
     long? expireAfter = null,
     MetaInfo? metaInfo = null,
     List<InstrumentConstraint>? constraints = null,
     DeviceContext? deviceContext = null,
     long? expireAt = null,
     string? deviceOS = null)
{
    public string MerchantOrderId { get; } = merchantOrderId;
    public long Amount { get; } = amount;
    public PgPaymentFlow PaymentFlow { get; } = paymentFlow as PgPaymentFlow ?? throw new ArgumentException("Invalid PaymentFlow type. Expected PgPaymentFlow.");
    public MetaInfo? MetaInfo { get; } = metaInfo;
    public List<InstrumentConstraint>? Constraints { get; } = constraints;
    public DeviceContext? DeviceContext { get; } = deviceContext;
    public long? ExpireAfter { get; } = expireAfter;
    public long? ExpireAt { get; } = expireAt;
    [JsonIgnore] public string? DeviceOS { get; } = deviceOS; // x-device-os header is required on for UPI_COLLECT requests as per the API design

    public static UpiIntentPayRequestBuilder UpiIntentPayRequestBuilder()
    {
        return new UpiIntentPayRequestBuilder();
    }

    public static UpiCollectPayViaVpaRequestBuilder UpiCollectPayViaVpaRequestBuilder()
    {
        return new UpiCollectPayViaVpaRequestBuilder();
    }

    public static UpiCollectPayViaPhoneNumberRequestBuilder UpiCollectPayViaPhoneNumberRequestBuilder()
    {
        return new UpiCollectPayViaPhoneNumberRequestBuilder();
    }

    public static UpiQrRequestBuilder UpiQrRequestBuilder()
    {
        return new UpiQrRequestBuilder();
    }

    public static NetBankingPayRequestBuilder NetBankingPayRequestBuilder()
    {
        return new NetBankingPayRequestBuilder();
    }

    public static TokenPayRequestBuilder TokenPayRequestBuilder()
    {
        return new TokenPayRequestBuilder();
    }

    public static CardPayRequestBuilder CardPayRequestBuilder()
    {
        return new CardPayRequestBuilder();
    }
}