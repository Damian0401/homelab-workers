namespace Homelab.Workers.Abstraction;

public class HomelabMqttWorkerOptions
{
    public required string Topic { get; init; }
    public required int EnsureConnectionLoopDelayInSeconds { get; init; }
}