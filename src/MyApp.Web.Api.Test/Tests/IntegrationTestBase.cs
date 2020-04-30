using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyApp.IntegrationTest.Tests
{
    [Trait("IntegrationTest", "true")]
    public class IntegrationTestBase : IClassFixture<MyAppWebApplicationFactory<Web.Api.Startup>>
    {
        private readonly MyAppWebApplicationFactory<Web.Api.Startup> _factory;

        protected IntegrationTestBase(MyAppWebApplicationFactory<Web.Api.Startup> factory)
        {
            _factory = factory;
        }

        protected HttpClient GetHttpClient()
        {
            var config = new FileConfiguration();
            var sut = config.SystemUnderTest;
            if (string.IsNullOrWhiteSpace(sut) || sut.ToLower() == "local")
                return _factory.CreateClient();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(sut);
            return httpClient;
        }

        protected async Task<T> PostAsync<T>(string path, object o)
        {
            var client = GetHttpClient();
            string json = JsonConvert.SerializeObject(o);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await client.PostAsync(path, content);
            var str = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }

        protected async Task<T> PutAsync<T>(string path, object o)
        {
            var client = GetHttpClient();
            string json = JsonConvert.SerializeObject(o);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await client.PutAsync(path, content);
            var str = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }

    public class FileConfiguration : Core.Configuration.IConfiguration
    {
        protected IConfigurationRoot _config;

        public FileConfiguration()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public string SystemUnderTest => _config[nameof(SystemUnderTest)];

        public string? EnvironmentName => _config[nameof(EnvironmentName)];
        public string ConnectionString => _config.GetConnectionString("MyAppDatabase");
        public string EsbConnectionString => _config["EsbConnectionString"];
    }
}
