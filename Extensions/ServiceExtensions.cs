using Cloudtenary.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cloudtenary.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCloudtenary(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(CloudtenarySettings));
            return services
                .Configure<CloudtenarySettings>(config)
                .AddSingleton<ICloudtenary>(provider =>
                {
                    var cloudSettings = provider.GetService<IOptions<CloudtenarySettings>>();
                    return new Cloudtenary(cloudSettings);
                });
        }
    }
}
