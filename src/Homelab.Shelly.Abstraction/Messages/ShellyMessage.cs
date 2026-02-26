using System.Text.Json;

namespace Homelab.Shelly.Abstraction.Messages;

public record ShellyMessage<T>
{
    public required string Src { get; init; }
    public required string Dst { get; init; }
    public required string Method { get; init; }
    public required T Params { get; init; }

    public static ShellyMessage<T> FromJsonPayload(ShellyMessage<JsonElement> message) => new()
    {
        Src = message.Src,
        Dst = message.Dst,
        Method = message.Method,
        Params = message.Params.Deserialize<T>(JsonSerializerOptions) 
                 ?? throw new InvalidOperationException()
    };
    
    // ReSharper disable once StaticMemberInGenericType
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}