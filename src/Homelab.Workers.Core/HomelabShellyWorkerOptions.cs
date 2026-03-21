using Homelab.Workers.Abstraction;

namespace Homelab.Workers.Core;

public class HomelabShellyWorkerOptions : HomelabMqttWorkerOptions
{
    public const string SectionName = "ShellyWorker";
    
    public required string DeviceId { get; init; }
    public required string DeviceType { get; init; }
    public required string RoomId { get; init; }
    public required string InfluxDbBucket { get; init; }
    public required bool IsTelemetryEnabled { get; init; }
}