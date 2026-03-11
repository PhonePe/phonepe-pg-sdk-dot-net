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
using NUnit.Framework;
using pg_sdk_dotnet.Common.Utils;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.Server;

namespace pg_sdk_dotnet.tests;

public abstract class BaseWireMockTest
{
    public const int WireMockPort = 30415;
    protected HttpClient HttpClient { get; private set; }
    protected WireMockServer wireMockServer;

    [OneTimeSetUp]
    public virtual async Task GlobalSetup()
    {
        wireMockServer = WireMockServer.Start(
            new WireMock.Settings.WireMockServerSettings
            {
                Port = WireMockPort,
                StartAdminInterface = true,
                ReadStaticMappings = false,
                Logger = new WireMockConsoleLogger(),
            }
        );
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri($"http://localhost:{WireMockPort}")
        };
        await Task.CompletedTask;
    }

    [OneTimeTearDown]
    public virtual async Task GlobalTeardown()
    {
        wireMockServer.Stop();
        wireMockServer.Dispose();

        Assert.That(wireMockServer.LogEntries, Is.Empty);

        await Task.CompletedTask;
    }

    [TearDown]
    public void TearDown()
    {
        wireMockServer.Reset();
    }

    protected void AddStubForGetRequest(string urlPath, Dictionary<string, string> queryParams, int status, Dictionary<string, string> requestHeaders, Dictionary<string, string> responseHeaders, object response)
    {
        var mappingBuilder = WireMock.RequestBuilders.Request.Create().WithPath(urlPath).UsingGet();
        foreach (var header in requestHeaders)
        {
            mappingBuilder = mappingBuilder.WithHeader(header.Key, header.Value);
        }
        foreach (var param in queryParams)
        {
            mappingBuilder = mappingBuilder.WithParam(param.Key, param.Value);
        }
        var responseBuilder = WireMock.ResponseBuilders.Response.Create().WithStatusCode(status).WithBody(JsonSerializer.Serialize(response));
        foreach (var header in responseHeaders)
        {
            responseBuilder = responseBuilder.WithHeader(header.Key, header.Value);
        }
        wireMockServer.Given(mappingBuilder).RespondWith(responseBuilder);
    }

    protected void AddStubForPostRequest(string urlPath, object request, int status, object response)
    {
        AddStubForPostRequest(urlPath, new Dictionary<string, string>(), request, status, new Dictionary<string, string>(), response);
    }

    protected void AddStubForPostRequest(string urlPath, Dictionary<string, string> requestHeaders, object request, int status, Dictionary<string, string> responseHeaders, object response)
    {
        var mappingBuilder = WireMock.RequestBuilders.Request.Create().WithPath(urlPath).UsingPost().WithBody(new JsonMatcher(JsonSerializer.Serialize(request, JsonOptions.IndentedWithPaymentConverters)));

        foreach (var header in requestHeaders)
        {
            mappingBuilder = mappingBuilder.WithHeader(header.Key, header.Value);
        }

        var responseBuilder = WireMock.ResponseBuilders.Response.Create().WithStatusCode(status).WithBody(JsonSerializer.Serialize(response));
        foreach (var header in responseHeaders)
        {
            responseBuilder = responseBuilder.WithHeader(header.Key, header.Value);
        }
        wireMockServer.Given(mappingBuilder).RespondWith(responseBuilder);
    }

    protected void AddStubForPostRequest(string urlPath, Dictionary<string, string> requestHeaders, string requestBody, int status, Dictionary<string, string> responseHeaders, object response)
    {
        var mappingBuilder = WireMock.RequestBuilders.Request.Create().WithPath(urlPath).UsingPost().WithBody(requestBody);

        foreach (var header in requestHeaders)
        {
            mappingBuilder = mappingBuilder.WithHeader(header.Key, header.Value);
        }

        var responseBuilder = WireMock.ResponseBuilders.Response.Create().WithStatusCode(status).WithBody(JsonSerializer.Serialize(response));

        foreach (var header in responseHeaders)
        {
            responseBuilder = responseBuilder.WithHeader(header.Key, header.Value);
        }

        wireMockServer.Given(mappingBuilder).RespondWith(responseBuilder);
    }

    protected void AddStubForFormDataPostRequest(string urlPath, Dictionary<string, string> requestHeaders, Dictionary<string, string> formData, int status, Dictionary<string, string> responseHeaders, object response)
    {
        string requestBody = FormDataToString(formData);
        AddStubForPostRequest(urlPath, requestHeaders, requestBody, status, responseHeaders, response);
    }

    protected string FormDataToString(Dictionary<string, string> formData)
    {
        return string.Join("&", formData.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
    }

}