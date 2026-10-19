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
using System.Text.Json.Serialization;

namespace pg_sdk_dotnet.Common.Models.Response.JsonConverters;

public class PaymentInstrumentConverter : JsonConverter<PaymentInstrumentV2>
{
    public override PaymentInstrumentV2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProperty)){
            throw new JsonException("Missing type property for PaymentInstrumentV2");
        }

        var type = typeProperty.GetString();
        return type switch
        {
            "ACCOUNT" or "CREDIT_LINE" => JsonSerializer.Deserialize<AccountInstrumentV2>(root.GetRawText(), options),
            "CREDIT_CARD" => JsonSerializer.Deserialize<CreditCardPaymentInstrumentV2>(root.GetRawText(), options),
            "DEBIT_CARD" => JsonSerializer.Deserialize<DebitCardPaymentInstrumentV2>(root.GetRawText(), options),
            "EGV" => JsonSerializer.Deserialize<EgvPaymentInstrumentV2>(root.GetRawText(), options),
            "NET_BANKING" => JsonSerializer.Deserialize<NetbankingPaymentInstrumentV2>(root.GetRawText(), options),
            "WALLET" => JsonSerializer.Deserialize<WalletPaymentInstrumentV2>(root.GetRawText(), options),
            _ => throw new JsonException($"Deseralization not possible because unknown type : {type} in PaymentInstrumentConverter")
        };
    }

    public override void Write(Utf8JsonWriter writer, PaymentInstrumentV2 value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
