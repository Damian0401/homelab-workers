namespace Homelab.InfluxDb.Abstraction;

public interface IHomelabInfluxDbClient
{
    Task SaveDataAsync(
        HomelabInfluxDbSaveOptions saveOptions,
        CancellationToken cancellationToken = default);
}

public record HomelabInfluxDbSaveOptions
{
    public required IDictionary<string, object> Fields { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string Measurement { get; init; }
    public required string Bucket { get; init; }
    public IDictionary<string, string>? Tags { get; init; }
}