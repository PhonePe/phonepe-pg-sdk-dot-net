# PhonePe PG .NET SDK Installation & Quick Start Guide

## Minimum Requirements

- **.NET Version**: `v8.0`

##  Installation

Install the dependency via the CLI:

```bash
dotnet add package phonepe-pg-sdk-dotnet --version 2.1.2
```

---

## Quick Start

To get your API keys, visit the **[PhonePe PG Merchant Onboarding Portal](https://developer.phonepe.com/v1/docs/merchant-onboarding)**.

You will need the following credentials:
- `clientId`
- `clientSecret`
- `clientVersion`

---

##  Class Initialization

Create an instance of `StandardCheckoutClient` using your credentials.

```csharp
using Microsoft.Extensions.Logging;
using pg_sdk_dotnet;
using pg_sdk_dotnet.Payments.v2;

// Initialize logger  
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

string clientId = "<clientId>";
string clientSecret = "<clientSecret>";
int clientVersion = 1; // Your client version

Env env = Env.SANDBOX; // Use Env.PRODUCTION in live mode

StandardCheckoutClient checkoutClient = StandardCheckoutClient.GetInstance(
    clientId,
    clientSecret,
    clientVersion,
    env,
    loggerFactory // null if you do not want logging enabled
);
```
> **Note:**  
> If you donot want logs from SDK pass null instead of loggerFactory as the 5th parameter in the checkoutClient.
---

## Initiate an Order Using Checkout Page

To initiate a payment, use the `StandardCheckoutPayRequest.Builder()` and invoke the `Pay` method.


### Code Example

```csharp
using pg_sdk_dotnet.Payments.v2;
using pg_sdk_dotnet.Payments.v2.Models.Request;
using pg_sdk_dotnet.Payments.v2.Models.Response;

var merchantOrderID = Guid.NewGuid().ToString();;
var amount= 100;
var redirectUrl = "https://www.merchant.com/redirect";

var payRequest = StandardCheckoutPayRequest.Builder()
    .SetMerchantOrderId(merchantOrderID)
    .SetAmount(amount)
    .SetRedirectUrl(redirectUrl)
    .Build();

StandardCheckoutPayResponse payResponse = await checkoutClient.Pay(payRequest);

Console.WriteLine("Redirect URL: " + payResponse.RedirectUrl);
```
---

## Check Status of an Order

Use `GetOrderStatus` to fetch the status of an order:

```csharp
using pg_sdk_dotnet;
using pg_sdk_dotnet.Payments.v2;

var merchantOrderID  = "<INSERT_ORDER_ID>"; // merchantOrderID passed in the Pay request

var orderStatusResponse = await checkoutClient.GetOrderStatus(merchantOrderID);
Console.WriteLine("Order Status: " + orderStatusResponse.State);
```

You can extract the `state` and other order details from the response object.

---

## Callback Handling

You will receive a callback from PhonePe on the configured URL in the dashboard. It is essential to validate the callback using `ValidateCallback`.

```csharp
using System.Text.Json;
using pg_sdk_dotnet.Common.Models.Response;
using pg_sdk_dotnet.Payments.v2;

string authorizationHeaderData = "ef4c914c591698b268db3c64163eafda7209a630f236ebf0eebf045460df723a"; // Received in the response headers
string phonepeS2SCallbackResponseBodyString = @"{
    ""event"": ""pg.order.completed"",
    ""payload"": { 
    }
}";

var usernameConfigured = "<MERCHANT_USERNAME>";
var passwordConfigured = "<MERCHANT_PASSWORD>";

CallbackResponse callbackResponse = checkoutClient.ValidateCallback(
    usernameConfigured,
    passwordConfigured,
    authorizationHeaderData,
    phonepeS2SCallbackResponseBodyString
);

var orderId = callbackResponse.payload.orderId;
var state = callbackResponse.payload.state;
```

>**Note:**  
> If the callback is invalid, `ValidateCallback` will throw a `PhonePeException`.

---

## Create SDK Order

Use `CreateSdkOrder` to initiate an order for SDK-based integration (e.g., native apps):

```csharp
using pg_sdk_dotnet.Common.Models.Request;
using pg_sdk_dotnet.Common.Models.Response;
using pg_sdk_dotnet.Payments.v2;
using pg_sdk_dotnet.Payments.v2.Models.Request;
using pg_sdk_dotnet.Payments.v2.Models.Response;

var redirectUrl = "https://www.merchant.com/redirect";
var merchantOrderID = Guid.NewGuid().ToString();;
var amount = 100;

var sdkOrderRequest = CreateSdkOrderRequest.StandardCheckoutBuilder()
    .SetMerchantOrderId(merchantOrderID)
    .SetAmount(amount)
    .SetDisablePaymentRetry(true)
    .SetRedirectUrl(redirectUrl)
    .Build();

CreateSdkOrderResponse createSdkOrderResponse = await checkoutClient.CreateSdkOrder(sdkOrderRequest);

var token = createSdkOrderResponse.Token;
```
The function returns a CreateSdkOrderResponse object. Merchant should retrieve the token from the received response.

## Documentation

For detailed API documentation, advanced features, and integration options:

- [.NET SDK Documentation](https://developer.phonepe.com/payment-gateway/backend-sdk/net-backend-sdk/introduction)
- [PhonePe Developer Portal](https://developer.phonepe.com/)

## Contributing

Contributions to the PhonePe PG SDK for PHP are welcome. Here's how you can contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure your code follows the project's coding standards and includes appropriate tests.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

```
Copyright 2025 PhonePe Private Limited

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```