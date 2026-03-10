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

namespace pg_sdk_dotnet.Common.Utils;

using pg_sdk_dotnet.Common.Models.Request.JsonConverters;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions CaseInsensitiveWithEnums = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static readonly JsonSerializerOptions IndentedWithPaymentConverters = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters = {
            new JsonStringEnumConverter(),
            new PaymentInstrumentConverter(),
            new PaymentRailConverter(),
            new PaymentModeConstraintConverter(),
            new CollectPaymentDetailsConverter(),
            new PaymentFlowJsonConverter()
        }
    };

    public static readonly JsonSerializerOptions PaymentInstrumentDeserialization = new()
    {
        Converters = { new PaymentInstrumentConverter() },
        PropertyNameCaseInsensitive = true
    };

    public static readonly JsonSerializerOptions PaymentRailDeserialization = new()
    {
        Converters = { new PaymentRailConverter() },
        PropertyNameCaseInsensitive = true
    };

    public static readonly JsonSerializerOptions PaymentModeConstraintDeserialization = new()
    {
        Converters = { new PaymentModeConstraintConverter() },
        PropertyNameCaseInsensitive = true
    };
    
    public static readonly JsonSerializerOptions IndentedWithRelaxedEscaping = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}

