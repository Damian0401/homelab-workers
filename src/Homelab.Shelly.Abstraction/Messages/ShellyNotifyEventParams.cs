namespace Homelab.Shelly.Abstraction.Messages;

public record ShellyNotifyEventParams : ShellyEntityWithTs
{
    public required IEnumerable<EventEntity> Events { get; init; }
    
    public record EventEntity : ShellyEntityWithTs
    {
        public required string Component { get; init; }
        public required string Event { get; init; }
    }
}