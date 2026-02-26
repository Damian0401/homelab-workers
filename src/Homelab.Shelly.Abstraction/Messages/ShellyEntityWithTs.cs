namespace Homelab.Shelly.Abstraction.Messages;

public record ShellyEntityWithTs
{
    public required double Ts { get; init; }
}