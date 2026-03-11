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
using pg_sdk_dotnet.Common.Models.Request.Instruments;

namespace pg_sdk_dotnet.Common.Models.Request.JsonConverters;

public class CollectPaymentDetailsConverter : JsonConverter<CollectPaymentDetails>
{
    public override CollectPaymentDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProperty))
            throw new JsonException("Missing type property for CollectPaymentDetails");

        return typeProperty.GetString() switch
        {
            "VPA" => JsonSerializer.Deserialize<VpaCollectPaymentDetails>(root.GetRawText(), options),
            "PHONE_NUMBER" => JsonSerializer.Deserialize<PhoneNumberCollectPaymentDetails>(root.GetRawText(), options),
            var t => throw new JsonException($"Unknown CollectPaymentDetails type: {t}")
        };
    }

    public override void Write(Utf8JsonWriter writer, CollectPaymentDetails value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var namingPolicy = options.PropertyNamingPolicy;
        string Prop(string name) => namingPolicy?.ConvertName(name) ?? name;

        writer.WriteStartObject();
        writer.WriteString(Prop("Type"), value.Type.ToString());

        switch (value)
        {
            case VpaCollectPaymentDetails vpa:
                writer.WriteString(Prop("Vpa"), vpa.Vpa);
                break;
            case PhoneNumberCollectPaymentDetails phone:
                writer.WriteString(Prop("PhoneNumber"), phone.PhoneNumber);
                break;
            default:
                throw new JsonException($"Unknown CollectPaymentDetails type: {value.GetType().Name}");
        }

        writer.WriteEndObject();
    }
}
