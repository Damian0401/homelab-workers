namespace Homelab.Shelly.Abstraction.Messages;

public record ShellyNotifyStatusParams : ShellyEntityWithTs
{
    public required MqttEntity Mqtt { get; init; }
    
    public record MqttEntity
    {
        public required bool Connected { get; init; }
    }
}