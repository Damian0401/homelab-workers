using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Homelab.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHomelabCommon(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);
        
        return services;
    }
}