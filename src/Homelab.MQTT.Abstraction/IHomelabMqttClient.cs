namespace Homelab.MQTT.Abstraction;

public interface IHomelabMqttClient : IAsyncDisposable
{
    bool IsConnected { get; }
    Task PublishAsync<T>(string topic, T payload, CancellationToken cancellationToken = default);
    Task SubscribeAsync(string topic, CancellationToken cancellationToken = default);
    void AddMessageHandler<T>(Func<T, Task> handler);
}