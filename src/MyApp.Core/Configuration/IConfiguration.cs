using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Configuration
{
    public interface IConfiguration
    {
        string? EnvironmentName { get; }

        string ConnectionString { get; }
        string EsbConnectionString { get; }
    }

    //Useful for unit tests 
    public class MockConfiguration : IConfiguration
    {
        public string? EnvironmentName { get; set; }
        public string ConnectionString { get; set; } = "";
        public string EsbConnectionString { get; set; } = "";
    }
}
