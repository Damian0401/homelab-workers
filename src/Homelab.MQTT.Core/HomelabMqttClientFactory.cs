using Homelab.MQTT.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace Homelab.MQTT.Core;

internal class HomelabMqttClientFactory(
    MqttClientFactory factory,
    IOptions<HomelabMqttOptions> options,
    ILogger<HomelabMqttClient> logger) 
    : IHomelabMqttClientFactory
{
    public IHomelabMqttClient CreateClient()
    {
        var client = factory.CreateMqttClient();
        return new HomelabMqttClient(client, options.Value, logger);
    }
}