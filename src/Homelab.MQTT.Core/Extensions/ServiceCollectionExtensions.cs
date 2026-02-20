using Homelab.MQTT.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MQTTnet;

namespace Homelab.MQTT.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHomelabMqtt(this IServiceCollection services)
    {
        services.AddOptions<MqttOptions>().BindConfiguration(MqttOptions.SectionName);
        services.TryAddSingleton<MqttClientFactory>();
        services.TryAddSingleton<IHomelabMqttClientFactory, HomelabMqttClientFactory>();
        
        return services;
    }
}