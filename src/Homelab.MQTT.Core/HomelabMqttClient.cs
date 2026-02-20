using System.Collections.Concurrent;
using System.Text.Json;
using Homelab.MQTT.Abstraction;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;

namespace Homelab.MQTT.Core;

internal class HomelabMqttClient 
    : IHomelabMqttClient
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttOptions _mqttOptions;
    private readonly ILogger<HomelabMqttClient> _logger;
    
    private readonly ConcurrentBag<string> _topics = new();
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public HomelabMqttClient(
        IMqttClient mqttClient,
        MqttOptions mqttOptions,
        ILogger<HomelabMqttClient> logger)
    {
        _mqttClient = mqttClient;
        _mqttOptions = mqttOptions;
        _logger = logger;
        
        _mqttClient.DisconnectedAsync += e =>
        {
            _logger.LogInformation("{Class}: Disconnected from MQTT broker. Reason: {Reason}.",
                nameof(HomelabMqttClient),
                e.ReasonString);
            return Task.CompletedTask;
        };
        _mqttClient.ConnectedAsync += async e =>
        {
            _logger.LogInformation("{Class}: Connected to MQTT broker. Reason: {Reason}",
                nameof(HomelabMqttClient),
                e.ConnectResult);

            if (e.ConnectResult.IsSessionPresent || _topics.IsEmpty)
            {
                return;
            }

            foreach (var topic in _topics)
            {
                await _mqttClient.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce);
            }
            
            _logger.LogInformation("{Class}: Subscribed to topics: {Topics}.", 
                nameof(HomelabMqttClient),
                string.Join(", ", _topics));
        };
    }
    
    public async Task SubscribeAsync(string topic, CancellationToken cancellationToken = default)
    {
        await EnsureClientConnection();

        if (_topics.Contains(topic))
        {
            _logger.LogWarning("{Class}: Topic {Topic} is already subscribed.",
                nameof(HomelabMqttClient),
                topic);
            return;
        }

        await _mqttClient.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce, cancellationToken);
        _topics.Add(topic);
        
        _logger.LogInformation("{Class}: Subscribed to topic {Topic}.", nameof(HomelabMqttClient), topic);
    }

    public void AddMessageHandler<T>(Func<T, Task> handler)
    {
        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            var content = e.ApplicationMessage.ConvertPayloadToString();
            
            if (typeof(T) == typeof(string))
            {
                await handler((T)(object)content);
                return;
            }

            try
            {
                var parsedContent = JsonSerializer.Deserialize<T>(content);
                if (parsedContent is null)
                {
                    return;
                }
                await handler(parsedContent);
            }
            catch (JsonException)
            {
                _logger.LogError("{Class}: Failed to deserialize message payload.", nameof(HomelabMqttClient));
            }
        };

        _logger.LogInformation("{Class}: Added message handler.", nameof(HomelabMqttClient));
    }

    public bool IsConnected => _mqttClient.IsConnected;

    public async Task PublishAsync<T>(string topic, T payload, CancellationToken cancellationToken = default)
    {
        await EnsureClientConnection();
        
        var payloadAsString = payload as string ?? JsonSerializer.Serialize(payload);
        
        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payloadAsString)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            .Build();
        
        await _mqttClient.PublishAsync(applicationMessage, cancellationToken);
        
        _logger.LogInformation("{Class}: Published message to topic {Topic}.", nameof(HomelabMqttClient), topic);
    }

    public async ValueTask DisposeAsync()
    {
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
        }
        
        _mqttClient.Dispose();
    }

    private async Task EnsureClientConnection()
    {
        await _semaphoreSlim.WaitAsync();

        try
        {
            if (_mqttClient.IsConnected)
            {
                return;
            }
        
            var connectOptions = BuildConnectOptions(_mqttOptions);
            await _mqttClient.ConnectAsync(connectOptions);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    
    private MqttClientOptions BuildConnectOptions(MqttOptions options) => new MqttClientOptionsBuilder()
        .WithTcpServer(options.Host, options.Port)
        .WithCredentials(options.Username, options.Password)
        .WithClientId(options.ClientId)
        .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
        .WithCleanSession(false)
        .WithSessionExpiryInterval(options.SessionExpiryIntervalInSeconds)
        .Build();
}