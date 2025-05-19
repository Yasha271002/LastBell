using Microsoft.Extensions.Hosting;

namespace LastBell.HostBuilders
{
    public static class BuildApiExtensions
    {
        public static IHostBuilder BuildApi(this IHostBuilder builder)
        {

            builder.ConfigureServices((context, services) =>
            {

            });
            return builder;
        }
    }
}