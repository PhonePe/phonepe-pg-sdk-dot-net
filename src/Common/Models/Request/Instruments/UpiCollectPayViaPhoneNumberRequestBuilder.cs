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

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;

public class UpiCollectPayViaPhoneNumberRequestBuilder
{
    private string _merchantOrderId = string.Empty;
    private long _amount;
    private MetaInfo? _metaInfo;
    private long? _expireAfter;
    private List<InstrumentConstraint>? _constraints;
    private string _phoneNumber = string.Empty;
    private string? _message;
    private string? _deviceOS;
    

    /**
     * SETTERS
     */

    public UpiCollectPayViaPhoneNumberRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetPhoneNumber(string phoneNumber)
    {
        this._phoneNumber = phoneNumber;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetMessage(string? message)
    {
        this._message = message;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetExpireAfter(long? expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public UpiCollectPayViaPhoneNumberRequestBuilder SetDeviceOS(string deviceOS)
    {
        this._deviceOS = deviceOS;
        return this;
    }

    public PgPaymentRequest Build()
    {
        var phoneNumberDetails = PhoneNumberCollectPaymentDetails.Builder()
            .SetPhoneNumber(this._phoneNumber)
            .Build();

        var paymentInstrument = CollectPaymentV2Instrument.Builder()
            .SetDetails(phoneNumberDetails)
            .SetMessage(this._message)
            .Build();

        var paymentFlow = PgPaymentFlow.Builder()
            .SetPaymentMode(paymentInstrument)
            .Build();

        return new PgPaymentRequest(
            this._merchantOrderId,
            this._amount,
            paymentFlow,
            this._expireAfter,
            this._metaInfo,
            this._constraints,
            deviceOS: this._deviceOS
        );
    }
}
