using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyApp.Core.Startup;
using Xunit;

namespace MyApp.Web.Api.Test.Startup
{
    public class ResolutionTest
    {
        [Fact]
        public void AddWebApi_Test()
        {
            var services = new ServiceCollection();
            services.AddWebApi();

            services.EjectAllInstancesOf<IConfiguration>();
            var config = new Mock<IConfiguration>();
            services.AddSingleton(config.Object);

            var msConfig = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(msConfig);

            services.AssertConfigurationIsValid();
        }
    }
}
