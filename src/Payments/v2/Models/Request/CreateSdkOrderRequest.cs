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

namespace pg_sdk_dotnet.Payments.v2.Models.Request;

public class CreateSdkOrderRequest
{
    public string MerchantOrderId { get; }
    public long Amount { get; }
    public PaymentFlow PaymentFlow { get; }
    public MetaInfo? MetaInfo { get; }
    public long? ExpireAfter { get; }
    public List<InstrumentConstraint>? Constraints { get; }
    public bool DisablePaymentRetry { get; }

    public CreateSdkOrderRequest(
        string merchantOrderId,
        long amount,
        PaymentFlow paymentFlow,
        MetaInfo? metaInfo,
        long? expireAfter,
        List<InstrumentConstraint>? constraints,
        bool disablePaymentRetry)
    {
        MerchantOrderId = merchantOrderId;
        Amount = amount;
        PaymentFlow = paymentFlow;
        MetaInfo = metaInfo;
        ExpireAfter = expireAfter;
        Constraints = constraints;
        DisablePaymentRetry = disablePaymentRetry;
    }

    public static StandardCheckoutBuilder StandardCheckoutBuilder()
    {
        return new StandardCheckoutBuilder();
    }

    public static CustomCheckoutBuilder CustomCheckoutBuilder()
    {
        return new CustomCheckoutBuilder();
    }
}

public class StandardCheckoutBuilder
{
    private string _merchantOrderId = string.Empty;
    private long _amount;
    private MetaInfo? _metaInfo;
    private string _message = string.Empty;
    private string _redirectUrl = string.Empty;
    private long? _expireAfter;
    private bool _disablePaymentRetry;

    public StandardCheckoutBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public StandardCheckoutBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public StandardCheckoutBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public StandardCheckoutBuilder SetMessage(string message)
    {
        this._message = message;
        return this;
    }

    public StandardCheckoutBuilder SetRedirectUrl(string redirectUrl)
    {
        this._redirectUrl = redirectUrl;
        return this;
    }

    public StandardCheckoutBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public StandardCheckoutBuilder SetDisablePaymentRetry(bool disablePaymentRetry)
    {
        this._disablePaymentRetry = disablePaymentRetry;
        return this;
    }

    public CreateSdkOrderRequest Build()
    {
        MerchantUrls MerchantUrls = MerchantUrls.Builder()
            .SetRedirectUrl(this._redirectUrl)
            .Build();

        PaymentFlow paymentFlow= PgCheckoutPaymentFlow.Builder()
            .SetMerchantUrls(MerchantUrls)
            .SetMessage(this._message)
            .Build();

        return new CreateSdkOrderRequest(
            this._merchantOrderId,
            this._amount,
            paymentFlow,
            this._metaInfo,
            this._expireAfter,
            null,
            this._disablePaymentRetry
        );
    }
}


public class CustomCheckoutBuilder
{
    private string _merchantOrderId = string.Empty;
    private long _amount;
    private MetaInfo? _metaInfo;
    private long? _expireAfter;
    private List<InstrumentConstraint>? _constraints;
    private bool _disablePaymentRetry;

    public CustomCheckoutBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public CustomCheckoutBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public CustomCheckoutBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public CustomCheckoutBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public CustomCheckoutBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public CustomCheckoutBuilder SetDisablePaymentRetry(bool disablePaymentRetry)
    {
        this._disablePaymentRetry = disablePaymentRetry;
        return this;
    }

    public CreateSdkOrderRequest Build()
    {
        PaymentFlow paymentFlow= PgPaymentFlow.Builder()
            .Build();

        return new CreateSdkOrderRequest(
            this._merchantOrderId,
            this._amount,
            paymentFlow,
            this._metaInfo,
            this._expireAfter,
            this._constraints,
            this._disablePaymentRetry
        );
    }
}
