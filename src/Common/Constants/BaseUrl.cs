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
public static class BaseUrl
{
    private static readonly Dictionary<Env, Dictionary<string, string>> Urls = new()
    {
        [Env.PRODUCTION] = new Dictionary<string, string>
        {
            [UrlConstants.PG_HOST_URL] = "https://api.phonepe.com/apis/pg",
            [UrlConstants.PCI_PG_HOST_URL] = "https://cards.phonepe.com/apis/pg",
            [UrlConstants.OAUTH_HOST_URL] = "https://api.phonepe.com/apis/identity-manager",
            [UrlConstants.EVENTS_HOST_URL] = "https://api.phonepe.com/apis/pg-ingestion"
        },
        [Env.SANDBOX] = new Dictionary<string, string>
        {
            [UrlConstants.PG_HOST_URL] = "https://api-preprod.phonepe.com/apis/pg-sandbox",
            [UrlConstants.PCI_PG_HOST_URL] = "https://api-preprod.phonepe.com/apis/pg-sandbox",
            [UrlConstants.OAUTH_HOST_URL] = "https://api-preprod.phonepe.com/apis/pg-sandbox",
            [UrlConstants.EVENTS_HOST_URL] = "http://localhost"
        },
        [Env.TESTING] = new Dictionary<string, string>
        {
            [UrlConstants.PG_HOST_URL] = "http://localhost:30415",
            [UrlConstants.PCI_PG_HOST_URL] = "http://localhost:30415",
            [UrlConstants.OAUTH_HOST_URL] = "http://localhost:30415",
            [UrlConstants.EVENTS_HOST_URL] = "http://localhost:30415"
        }
    };

    public static string GetUrl(Env environment, string key) =>
        Urls[environment].TryGetValue(key, out var url) ? url : throw new KeyNotFoundException($"Key {key} not found.");
}