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

using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[TestFixture]
[NonParallelizable]
public class CustomCheckoutClientTests : BaseSetupWithOAuth
{
    private CustomCheckoutClient customCheckoutClient;
    private Dictionary<string, string> FormData;


    [SetUp] 
    public void Setup()
    {
        var env = Env.TESTING;

        customCheckoutClient = CustomCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            env
        );

        FormData = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "client_version", ClientVersion.ToString() },
            { "grant_type", "client_credentials" }
        };
    }
}

