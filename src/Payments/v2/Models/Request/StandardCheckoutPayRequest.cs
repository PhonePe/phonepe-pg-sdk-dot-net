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
public class StandardCheckoutPayRequest
{
    public string MerchantOrderId { get; }
    public long Amount { get; }
    public PaymentFlow PaymentFlow { get; }
    public MetaInfo? MetaInfo { get; }
    public long? ExpireAfter { get; }
    public bool? DisablePaymentRetry { get; }
    public PrefillUserLoginDetails? PrefillUserLoginDetails { get; }

    public StandardCheckoutPayRequest(
        string merchantOrderId,
        long amount,
        MetaInfo? metaInfo,
        string? message,
        string? redirectUrl,
        PaymentModeConfig? paymentModeConfig,
        long? expireAfter,
        bool? disablePaymentRetry = null,
        PrefillUserLoginDetails? prefillUserLoginDetails = null)
    {
        MerchantOrderId = merchantOrderId;
        Amount = amount;
        MetaInfo = metaInfo;
        ExpireAfter = expireAfter;
        DisablePaymentRetry = disablePaymentRetry;
        PrefillUserLoginDetails = prefillUserLoginDetails;

        var merchantUrls = MerchantUrls.Builder()
            .SetRedirectUrl(redirectUrl)
            .Build();

        PaymentFlow = PgCheckoutPaymentFlow.Builder()
            .SetMerchantUrls(merchantUrls)
            .SetMessage(message)
            .SetPaymentModeConfig(paymentModeConfig)
            .Build();
    }

    public static StandardCheckoutPayRequestBuilder Builder()
    {
        return new StandardCheckoutPayRequestBuilder();
    }
}


public class StandardCheckoutPayRequestBuilder
{
    private string _merchantOrderId = string.Empty;
    private long _amount;
    private MetaInfo? _metaInfo;
    private string? _redirectUrl;
    private string? _message;
    private PaymentModeConfig? _paymentModeConfig;
    private long? _expireAfter;
    private bool? _disablePaymentRetry;
    private PrefillUserLoginDetails? _prefillUserLoginDetails;

    public StandardCheckoutPayRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetRedirectUrl(string redirectUrl)
    {
        this._redirectUrl = redirectUrl;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetMessage(string message)
    {
        this._message = message;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetPaymentModeConfig(PaymentModeConfig paymentModeConfig)
    {
        this._paymentModeConfig = paymentModeConfig;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetDisablePaymentRetry(bool disablePaymentRetry)
    {
        this._disablePaymentRetry = disablePaymentRetry;
        return this;
    }

    public StandardCheckoutPayRequestBuilder SetPrefillUserLoginDetails(PrefillUserLoginDetails? prefillUserLoginDetails)
    {
        this._prefillUserLoginDetails = prefillUserLoginDetails;
        return this;
    }

    public StandardCheckoutPayRequest Build()
    {
        return new StandardCheckoutPayRequest(
            this._merchantOrderId,
            this._amount,
            this._metaInfo,
            this._message,
            this._redirectUrl,
            this._paymentModeConfig,
            this._expireAfter,
            this._disablePaymentRetry,
            this._prefillUserLoginDetails
        );
    }
}
