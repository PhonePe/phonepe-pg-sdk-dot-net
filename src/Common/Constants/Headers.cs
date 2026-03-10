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

namespace pg_sdk_dotnet.Common.Constants;
public class Headers
{
  public static readonly string INTEGRATION = "API";
  public static readonly string API_VERSION = "V2";
  public static readonly string SDK_VERSION = "2.1.2";
  public static readonly string SDK_TYPE = "BACKEND_DOTNET_SDK";
  public static readonly string SOURCE = "x-source";
  public static readonly string SOURCE_VERSION = "x-source-version";
  public static readonly string SOURCE_PLATFORM = "x-source-platform";
  public static readonly string SOURCE_PLATFORM_VERSION = "x-source-platform-version";
  public static readonly string OAUTH_AUTHORIZATION = "Authorization";
  public static readonly string CONTENT_TYPE = "Content-Type";
  public static readonly string ACCEPT = "Accept";
  public static readonly string APPLICATION_JSON = "application/json";
  public static readonly string X_DEVICE_OS = "x-device-os";
  public static readonly string APPLICATION_FORM_URLENCODED =
    "application/x-www-form-urlencoded";
}
