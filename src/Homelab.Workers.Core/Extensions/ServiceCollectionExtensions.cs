using Microsoft.Extensions.DependencyInjection;

namespace Homelab.Workers.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHomelabWorkers(this IServiceCollection services)
    {
        services.AddOptions<HomelabShellyWorkerOptions>()
            .BindConfiguration(HomelabShellyWorkerOptions.SectionName);

        services.AddHostedService<HomelabShellyWorker>();
        
        return services;
    }
}