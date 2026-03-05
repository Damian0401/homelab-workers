namespace Homelab.InfluxDb.Core;

internal record HomelabInfluxDbOptions
{
    public const string SectionName = "InfluxDb";
    
    public required string Host { get; init; }
    public required string Token { get; init; }
    public required string Organization { get; init; }
}