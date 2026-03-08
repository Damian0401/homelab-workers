namespace Homelab.Workers.Core.Models;

internal record HomelabMeasurementDto
{
    public int BatteryPercent { get; init; }
    public double BatteryVoltage { get; init; }
    public double TemperatureC { get; init; }
    public double RelativeHumidity { get; init; }
    public int WifiRssi { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}