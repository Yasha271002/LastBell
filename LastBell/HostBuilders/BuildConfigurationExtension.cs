﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LastBell.HostBuilders
{
    public static class BuildConfigurationExtension
    {
        public static IHostBuilder BuildConfiguration(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(c =>
            {
                c.AddJsonFile("appsettings.json");
                c.AddEnvironmentVariables();
            });
            return builder;
        }
    }
}
