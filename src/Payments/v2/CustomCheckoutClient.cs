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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace pg_sdk_dotnet.Payments.v2;

public class CustomCheckoutClient : BaseClient
{
    private static CustomCheckoutClient? _client;
    private static readonly object _lock = new();
    private readonly ILogger<CustomCheckoutClient> _logger ;
    private readonly Dictionary<string, string> _headers;

    private CustomCheckoutClient(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory
        )
        : base(clientId, clientSecret, clientVersion, env, loggerFactory ?? NullLoggerFactory.Instance)
    {
        this._logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<CustomCheckoutClient>();
        this._headers = PrepareHeaders();

    }

    /*
     * Returns a singleton instance of CustomCheckoutClient.
     * Ensures only one instance is created with the same parameters.
     */
    public static CustomCheckoutClient GetInstance(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory = null
    )
    {
        return new CustomCheckoutClient(clientId, clientSecret, clientVersion, env, loggerFactory);
    }

    /*
    * Initiates a custom checkout payment.
    */
    public async Task<CustomCheckoutPayResponse> Pay(PgPaymentRequest payRequest)
    {
        var url = CustomCheckoutConstants.PAY_API;

        try 
        {
            var response =  await RequestViaAuthRefreshAsync<CustomCheckoutPayResponse, PgPaymentRequest>(
                HttpMethodType.POST,
                url,
                this._headers,
                Headers.APPLICATION_JSON,
                payRequest
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Pay API Failed");
            throw;
        }
    }


    /*
    * Fetches the order status.
    */
    public async Task<OrderStatusResponse> GetOrderStatus(string merchantOrderId, bool details = false)
    {
        var url = CustomCheckoutConstants.ORDER_STATUS_API.Replace("{ORDER_ID}", merchantOrderId);
        var queryParams = new Dictionary<string, string> { { CustomCheckoutConstants.ORDER_DETAILS, details.ToString().ToLower() } };
    
        try
        {
            var response = await RequestViaAuthRefreshAsync<OrderStatusResponse, object>(
                HttpMethodType.GET,
                url,
                this._headers,
                Headers.APPLICATION_JSON,
                null,
                queryParams
            );

            return response;

        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Order Status API Failed");
            throw;
        }
    }

    /*
    * Initiates a refund for a completed order.
    */
    public async Task<RefundResponse> Refund(RefundRequest refundRequest)
    {
        var url = CustomCheckoutConstants.REFUND_API;

        try
        {
            var response = await RequestViaAuthRefreshAsync<RefundResponse, RefundRequest>(
                HttpMethodType.POST,
                url,
                this._headers,
                Headers.APPLICATION_JSON,
                refundRequest
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Refund API Failed");
            throw;
        }
    }

    /*
    * Fetches the status of a refund.
    */
    public async Task<RefundStatusResponse> GetRefundStatus(string refundId)
    {
        var url = CustomCheckoutConstants.REFUND_STATUS_API.Replace("{REFUND_ID}", refundId);

        try
        {
            var response = await RequestViaAuthRefreshAsync<RefundStatusResponse, object>(
                HttpMethodType.GET,
                url,
                this._headers,
                Headers.APPLICATION_JSON
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Refund Status API Failed");
            throw;
        }
    }

    /*
    * Fetches the status of a transaction.
    */
    public async Task<OrderStatusResponse> GetTransactionStatus(string transactionId)
    {
        var url = CustomCheckoutConstants.TRANSACTION_STATUS_API.Replace("{TRANSACTION_ID}", transactionId);

        try
        {
            var response = await RequestViaAuthRefreshAsync<OrderStatusResponse, object>(
                HttpMethodType.GET,
                url,
                this._headers,
                Headers.APPLICATION_JSON
            );
    
            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Transaction Status API Failed");
            throw;
        }
    }

    /*
    * Creates an SDK order for integration.
    */
    public async Task<CreateSdkOrderResponse> CreateSdkOrder(CreateSdkOrderRequest sdkRequest)
    {
        var url = CustomCheckoutConstants.CREATE_ORDER_API;

        try
        {
            var response = await RequestViaAuthRefreshAsync<CreateSdkOrderResponse, CreateSdkOrderRequest>(
                HttpMethodType.POST,
                url,
                this._headers,
                Headers.APPLICATION_JSON,
                sdkRequest
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Create SDK Order API Failed");
            throw;
        }
    }

    /*
    * Validates the callback response.
    */
    public CallbackResponse ValidateCallback(
        string username,
        string password,
        string authorization,
        string responseBody)
    {
        if (!CommonUtils.IsCallbackValid(username, password, authorization))
        {
            throw new PhonePeException(417, "Invalid Callback");
        }

        var callbackResponse = JsonSerializer.Deserialize<CallbackResponse>(responseBody, JsonOptions.CaseInsensitiveWithEnums)
            ?? throw new PhonePeException(500, "Invalid Callback Response");

        return callbackResponse;
    }


    /*
     * Prepares default headers.
     */
    private static Dictionary<string, string> PrepareHeaders()
    {
        return new Dictionary<string, string>
        {
            { Headers.CONTENT_TYPE, Headers.APPLICATION_JSON }, 
            { Headers.SOURCE, Headers.INTEGRATION },
            { Headers.SOURCE_VERSION, Headers.API_VERSION},
            { Headers.SOURCE_PLATFORM, Headers.SDK_TYPE},
            {Headers.SOURCE_PLATFORM_VERSION, Headers.SDK_VERSION}
        };
    }
}