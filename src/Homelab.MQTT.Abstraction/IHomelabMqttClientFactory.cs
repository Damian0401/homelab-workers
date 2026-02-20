namespace Homelab.MQTT.Abstraction;

public interface IHomelabMqttClientFactory
{
    IHomelabMqttClient CreateClient();
}