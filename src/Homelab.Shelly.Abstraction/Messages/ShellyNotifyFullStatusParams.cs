using System.Text.Json.Serialization;

namespace Homelab.Shelly.Abstraction.Messages;

public record ShellyNotifyFullStatusParams : ShellyEntityWithTs
{
    public required object Ble { get; init; }
    public required CloudEntity Cloud { get; init; }
    [JsonPropertyName("devicepower:0")]
    public required DevicePowerEntity DevicePowerZero { get; init; }
    public required object HtUi { get; init; }
    [JsonPropertyName("humidity:0")]
    public required HumidityEntity HumidityZero { get; init; }
    public required MqttEntity Mqtt { get; init; }
    public required SysEntity Sys { get; init; }
    [JsonPropertyName("temperature:0")]
    public required TemperatureEntity TemperatureZero { get; init; }
    public required WifiEntity Wifi { get; init; }
    public required WsEntity Ws { get; init; }

    public record CloudEntity
    {
        public required bool Connected { get; init; }
    }
    
    public record DevicePowerEntity
    {
        public required int Id { get; init; }
        public required BatteryEntity Battery { get; init; }
        public required ExternalEntity External { get; init; }
        
        public record BatteryEntity
        {
            [JsonPropertyName("V")]
            public required double V { get; init; }
            public required int Percent { get; init; }
        }
        
        public record ExternalEntity
        {
            public required bool Present { get; init; }
        }
    }
    
    public record HumidityEntity
    {
        public required int Id { get; init; }
        public required double Rh { get; init; }
    }
    
    public record MqttEntity
    {
        public required bool Connected { get; init; }
    }

    public record SysEntity
    {
        public required string Mac { get; init; }
        public required bool RestartRequired { get; init; }
        public required object? Time { get; init; }
        public required object? Unixtime { get; init; }
        public required int Uptime { get; init; }
        public required int RamSize { get; init; }
        public required int RamFree { get; init; }
        public required int FsSize { get; init; }
        public required int FsFree { get; init; }
        public required int CfgRev { get; init; }
        public required int KvsRev { get; init; }
        public required int WebhookRev { get; init; }
        public required object AvailableUpdates { get; init; }
        public required WakeupReasonEntity WakeupReason { get; init; }
        public required int WakeupPeriod { get; init; }
        public required int ResetReason { get; init; }

        public record WakeupReasonEntity
        {
            public required string Boot { get; init; }
            public required string Cause { get; init; }
        }
    }

    public record TemperatureEntity
    {
        public required int Id { get; init; }
        [JsonPropertyName("tC")]
        public required double Tc { get; init; }
        [JsonPropertyName("tF")]
        public required double Tf { get; init; }
    }

    public record WifiEntity
    {
        public required string StaIp { get; init; }
        public required string Status { get; init; }
        public required string Ssid { get; init; }
        public required int Rssi { get; init; }
    }
    
    public record WsEntity
    {
        public required bool Connected { get; init; }
    }
}