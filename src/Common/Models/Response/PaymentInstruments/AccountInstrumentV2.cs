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

namespace pg_sdk_dotnet.Common.Models.Response.PaymentInstruments;

public class AccountInstrumentV2 : PaymentInstrumentV2
{
    public override required PaymentInstrumentType Type { get; set; } = PaymentInstrumentType.ACCOUNT;
    public string? MaskedAccountNumber { get; set; }
    public string? Ifsc { get; set; }
    public string? AccountHolderName { get; set; }
    public string? AccountType { get; set; }
    public string? UnmaskedAccountNumber { get; set; }
    public string? ProviderAccountType { get; set; }
}
