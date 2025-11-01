using Cloudtenary.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudtenary.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCloudtenary(this ServiceCollection services, 
                                                        Action<CloudtenarySettings> options)
        {
            var settings = new CloudtenarySettings();
            options(settings);

            return services
                .AddSingleton<ICloudtenary>(provider =>
                {
                    return new Cloudtenary(settings);
                });
        }
    }
}
