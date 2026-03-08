using Homelab.MQTT.Abstraction;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Homelab.Workers.Abstraction;

public abstract class HomelabMqttWorker<TMessage, TOptions>(
    IHomelabMqttClientFactory mqttClientFactory,
    TimeProvider timeProvider,
    TOptions options,
    ILogger logger) 
    : BackgroundService
    where TOptions : HomelabMqttWorkerOptions
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("{Class}: Worker started at: {Date}",
                nameof(HomelabMqttWorker<,>),
                timeProvider.GetUtcNow());
        }
        
        await using var client = mqttClientFactory.CreateClient(); 
        
        client.AddMessageHandler<TMessage>(OnMessageReceivedAsync);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await EnsureMqttConnectionAsync(client, cancellationToken);

                await Task.Delay(
                    TimeSpan.FromSeconds(options.EnsureConnectionLoopDelayInSeconds),
                    cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("{Class}: Worker stopped at: {Date}",
                    nameof(HomelabMqttWorker<,>),
                    timeProvider.GetUtcNow());
            }
        }
    }

    private async Task EnsureMqttConnectionAsync(IHomelabMqttClient client, CancellationToken cancellationToken)
    {
        if (client.IsConnected)
        {
            return;
        }
        
        try
        {
            await client.SubscribeAsync(options.Topic, cancellationToken);
        }
        catch (Exception)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("{Class}: Failed to connect to MQTT broker.",
                    nameof(HomelabMqttWorker<,>));
            }
        }
    }

    protected abstract Task OnMessageReceivedAsync(TMessage message);
}