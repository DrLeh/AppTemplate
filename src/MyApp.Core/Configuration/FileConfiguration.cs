using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyApp.Core.Configuration
{
    public class FileConfiguration : IConfiguration
    {
        public Microsoft.Extensions.Configuration.IConfiguration _config;

        public FileConfiguration(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _config = config;
        }

        public string? EnvironmentName => _config[nameof(EnvironmentName)];
        public string ApplicationCreationSecret => _config[nameof(ApplicationCreationSecret)];
        public string ConnectionString => _config.GetConnectionString("MyAppDatabase");
        public string EsbConnectionString => _config["EsbConnectionString"];
    }
}
