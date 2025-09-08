# PhonePe B2B Payment Gateway SDK for Java .NET

The PhonePe Payment Gateway .NET SDK provides a convenient way to integrate with the PhonePe Payment Gateway APIs.

## Requirements

*   .NET 6.0 or later

## Installation

You can install the SDK using the .NET CLI:

```bash
dotnet add package phonepe-pg-sdk-dotnet --version 2.0.2
```


## Quick Start

### Initialization

Before you can use the SDK, you need to get your `ClientId`, `ClientSecret`, and `ClientVersion` from the PhonePe Merchant Portal.


```csharp
using pg_sdk_dotnet.Payments.v2;
using pg_sdk_dotnet;

var standardCheckoutClient = StandardCheckoutClient.GetInstance(
    clientId: "YOUR_CLIENT_ID",
    clientSecret: "YOUR_CLIENT_SECRET",
    clientVersion: 1,
    env: Env.UAT // Or Env.PROD for production
);
```

### Standard Checkout Flow

#### Initiate a Payment

To start a payment, create a `StandardCheckoutPayRequest` and call the `Pay` method. The response will contain a `CheckoutPageUrl` to which you can redirect the user.

```csharp
using pg_sdk_dotnet.Payments.v2.Models.Request;
using pg_sdk_dotnet.Common.Models;

var merchantOrderID = Guid.NewGuid().ToString();
var metaInfo = MetaInfo.builder()
                    .udf1("udf1")
                    .udf2("udf2")
                    .build();
var payRequest = StandardCheckoutPayRequest.Builder()
    .SetMerchantOrderId(merchantOrderID)
    .SetAmount(10000) // Amount in paise
    .SetRedirectUrl("https://your-domain.com/payment-redirect")
    .SetMessage("Payment for order M-123456789")
    .SetMetaInfo(metaInfo)
    .Build();

StandardCheckoutPayResponse response = await checkoutClient.Pay(payRequest);
logger.LogInformation("Pay API Response:\n{Response}", JsonSerializer.Serialize(response, JsonOptions.IndentedWithRelaxedEscaping));
```

#### Check Order Status

You can check the status of an order using the `GetOrderStatus` method.

```csharp
var response = await checkoutClient.GetOrderStatus(orderId, details: true);
```

### Refunds

You can initiate a refund for a completed transaction.

#### Initiate a Refund

```csharp
using pg_sdk_dotnet.Common.Models.Request;

var refundRequest = new RefundRequest(
    "M-123456789",
    "R-123456789",
    10000 // Amount in paise
);

try
{
    var response = await standardCheckoutClient.Refund(refundRequest);
    // Handle response
}
catch (PhonePeException ex)
{
    // Handle exception
}
```

#### Check Refund Status

```csharp
try
{
    var refundStatus = await standardCheckoutClient.GetRefundStatus("R-123456789");
    // Process refundStatus
}
catch (PhonePeException ex)
{
    // Handle exception
}
```

### Handling Callbacks

The SDK provides a way to validate callbacks from PhonePe to ensure they are authentic.

```csharp
// Assuming you have the following from the callback
string username = "YOUR_CALLBACK_USERNAME";
string password = "YOUR_CALLBACK_PASSWORD";
string authorizationHeader = "Basic ..."; // From the Authorization header
string responseBody = "{...}"; // The JSON body of the callback

try
{
    var callbackResponse = standardCheckoutClient.ValidateCallback(
        username,
        password,
        authorizationHeader,
        responseBody
    );
    // Process the validated callbackResponse
}
catch (PhonePeException ex)
{
    // Handle invalid callback
}
```
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