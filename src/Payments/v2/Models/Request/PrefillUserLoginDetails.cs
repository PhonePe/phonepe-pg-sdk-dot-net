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

public class PrefillUserLoginDetails
{
    public string? PhoneNumber { get; }

    public PrefillUserLoginDetails(string? phoneNumber = null)
    {
        PhoneNumber = phoneNumber;
    }

    public static PrefillUserLoginDetailsBuilder Builder()
    {
        return new PrefillUserLoginDetailsBuilder();
    }
}

public class PrefillUserLoginDetailsBuilder
{
    private string? _phoneNumber;

    public PrefillUserLoginDetailsBuilder SetPhoneNumber(string phoneNumber)
    {
        this._phoneNumber = phoneNumber;
        return this;
    }

    public PrefillUserLoginDetails Build()
    {
        if (this._phoneNumber != null)
        {
            if (this._phoneNumber.Length == 0)
                throw new ArgumentException("PhoneNumber must not be empty.", nameof(_phoneNumber));
        }

        return new PrefillUserLoginDetails(this._phoneNumber);
    }
}
