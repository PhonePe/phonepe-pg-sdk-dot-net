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

namespace pg_sdk_dotnet.Common.Models;

public class MetaInfo(string? udf1, string? udf2, string? udf3, string? udf4, string? udf5, 
                      string? udf6, string? udf7, string? udf8, string? udf9, string? udf10, 
                      string? udf11, string? udf12, string? udf13, string? udf14, string? udf15)
{
    public string? Udf1 { get; private set; } = udf1;
    public string? Udf2 { get; private set; } = udf2;
    public string? Udf3 { get; private set; } = udf3;
    public string? Udf4 { get; private set; } = udf4;
    public string? Udf5 { get; private set; } = udf5;
    public string? Udf6 { get; private set; } = udf6;
    public string? Udf7 { get; private set; } = udf7;
    public string? Udf8 { get; private set; } = udf8;
    public string? Udf9 { get; private set; } = udf9;
    public string? Udf10 { get; private set; } = udf10;
    public string? Udf11 { get; private set; } = udf11;
    public string? Udf12 { get; private set; } = udf12;
    public string? Udf13 { get; private set; } = udf13;
    public string? Udf14 { get; private set; } = udf14;
    public string? Udf15 { get; private set; } = udf15;

    public static MetaInfoBuilder Builder()
    {
        return new MetaInfoBuilder();
    }

    public class MetaInfoBuilder
    {
        private string? _udf1;
        private string? _udf2;
        private string? _udf3;
        private string? _udf4;
        private string? _udf5;
        private string? _udf6;
        private string? _udf7;
        private string? _udf8;
        private string? _udf9;
        private string? _udf10;
        private string? _udf11;
        private string? _udf12;
        private string? _udf13;
        private string? _udf14;
        private string? _udf15;

        public MetaInfoBuilder SetUdf1(string udf1)
        {
            this._udf1 = udf1;
            return this;
        }

        public MetaInfoBuilder SetUdf2(string udf2)
        {
            this._udf2 = udf2;
            return this;
        }

        public MetaInfoBuilder SetUdf3(string udf3)
        {
            this._udf3 = udf3;
            return this;
        }

        public MetaInfoBuilder SetUdf4(string udf4)
        {
            this._udf4 = udf4;
            return this;
        }

        public MetaInfoBuilder SetUdf5(string udf5)
        {
            this._udf5 = udf5;
            return this;
        }
        public MetaInfoBuilder SetUdf6(string udf6)
        {
            this._udf6 = udf6;
            return this;
        }
        public MetaInfoBuilder SetUdf7(string udf7)
        {
            this._udf7 = udf7;
            return this;
        }
        public MetaInfoBuilder SetUdf8(string udf8)
        {
            this._udf8 = udf8;
            return this;
        }
        public MetaInfoBuilder SetUdf9(string udf9)
        {
            this._udf9 = udf9;
            return this;
        }
        public MetaInfoBuilder SetUdf10(string udf10)
        {
            this._udf10 = udf10;
            return this;
        }
        public MetaInfoBuilder SetUdf11(string udf11)
        {
            this._udf11 = udf11;
            return this;
        }
        public MetaInfoBuilder SetUdf12(string udf12)
        {
            this._udf12 = udf12;
            return this;
        }
        public MetaInfoBuilder SetUdf13(string udf13)
        {
            this._udf13 = udf13;
            return this;
        }
        public MetaInfoBuilder SetUdf14(string udf14)
        {
            this._udf14 = udf14;
            return this;
        }
        public MetaInfoBuilder SetUdf15(string udf15)
        {
            this._udf15 = udf15;
            return this;
        }

        public MetaInfo Build()
        {
            ValidateSize("Udf1", _udf1, 256);
            ValidateSize("Udf2", _udf2, 256);
            ValidateSize("Udf3", _udf3, 256);
            ValidateSize("Udf4", _udf4, 256);
            ValidateSize("Udf5", _udf5, 256);
            ValidateSize("Udf6", _udf6, 256);
            ValidateSize("Udf7", _udf7, 256);
            ValidateSize("Udf8", _udf8, 256);
            ValidateSize("Udf9", _udf9, 256);
            ValidateSize("Udf10", _udf10, 256);
            ValidateSizeAndPattern("Udf11", _udf11, 50);
            ValidateSizeAndPattern("Udf12", _udf12, 50);
            ValidateSizeAndPattern("Udf13", _udf13, 50);
            ValidateSizeAndPattern("Udf14", _udf14, 50);
            ValidateSizeAndPattern("Udf15", _udf15, 50);
            return new MetaInfo(this._udf1, this._udf2, this._udf3, this._udf4, this._udf5, this._udf6, this._udf7, this._udf8, this._udf9, this._udf10, this._udf11, this._udf12, this._udf13, this._udf14, this._udf15);
        }

        private static void ValidateSize(string field, string? value, int max)
        {
            if (value != null && value.Length > max)
                throw new ArgumentException($"{field} exceeds maximum allowed size of {max} characters");
        }

        private static readonly System.Text.RegularExpressions.Regex RestrictedPattern =
            new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_\- @.+]*$");

        private static void ValidateSizeAndPattern(string field, string? value, int max)
        {
            if (value == null) return;
            if (value.Length > max)
                throw new ArgumentException($"{field} exceeds maximum allowed size of {max} characters");
            if (!RestrictedPattern.IsMatch(value))
                throw new ArgumentException(
                    $"{field} should only contain alphanumeric characters, underscores, hyphens, spaces, @, ., and +");
        }
    }
}