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

namespace pg_sdk_dotnet;
public class EnvConfig
{
    public Env EnvType { get; }
    public string PgHostUrl { get; }
    public string PciPgHostUrl { get; }
    public string OAuthHostUrl { get; }
    public string EventsHostUrl { get; }

    private EnvConfig(Env envType, string pgHostUrl, string pciPgHostUrl, string oAuthHostUrl, string eventsHostUrl)
    {
        EnvType = envType;
        PgHostUrl = pgHostUrl;
        PciPgHostUrl = pciPgHostUrl;
        OAuthHostUrl = oAuthHostUrl;
        EventsHostUrl = eventsHostUrl;
    }

    public static EnvConfig GetBaseUrls(Env envType)
    {
        try
        {
            return new EnvConfig(
                envType,
                BaseUrl.GetUrl(envType, UrlConstants.PG_HOST_URL),
                BaseUrl.GetUrl(envType, UrlConstants.PCI_PG_HOST_URL),
                BaseUrl.GetUrl(envType, UrlConstants.OAUTH_HOST_URL),
                BaseUrl.GetUrl(envType, UrlConstants.EVENTS_HOST_URL)
            );
        }
        catch (KeyNotFoundException)
        {
            return new EnvConfig(
                Env.SANDBOX,
                BaseUrl.GetUrl(Env.SANDBOX, UrlConstants.PG_HOST_URL),
                BaseUrl.GetUrl(Env.SANDBOX, UrlConstants.PCI_PG_HOST_URL),
                BaseUrl.GetUrl(Env.SANDBOX, UrlConstants.OAUTH_HOST_URL),
                BaseUrl.GetUrl(Env.SANDBOX, UrlConstants.EVENTS_HOST_URL)
            );
        }
    }
}
