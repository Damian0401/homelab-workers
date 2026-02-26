namespace Homelab.MQTT.Core;

public record MqttOptions
{
    public const string SectionName = "Mqtt";
    
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string ClientId { get; init; }
    public required uint SessionExpiryIntervalInSeconds { get; init; }
    public required string MessageJsonNamingPolicy { get; init; }
}