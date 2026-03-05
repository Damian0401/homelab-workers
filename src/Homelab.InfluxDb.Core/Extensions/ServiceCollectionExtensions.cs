using Homelab.InfluxDb.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Homelab.InfluxDb.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHomelabInfluxDb(this IServiceCollection services)
    {
        services.AddOptions<HomelabInfluxDbOptions>().BindConfiguration(HomelabInfluxDbOptions.SectionName);
        services.TryAddSingleton<IHomelabInfluxDbClient, HomelabInfluxDbClient>();
        
        return services;
    }
}