namespace Homelab.Workers.Core.Constants;

public static class HomelabMeasurements
{
    public static class Environment
    {
        public const string Name = "environment";

        public static class Fields
        {
            public const string TemperatureC = "temperature_c";
            public const string RelativeHumidity = "relative_humidity";
        }
    }
        
    public static class Telemetry
    {
        public const string Name = "telemetry";
            
        public static class Fields
        {
            public const string BatteryPercent = "battery_percent";
            public const string BatteryVoltage = "battery_voltage";
            public const string WifiRssi = "wifi_rssi";
        }
    }
}

public static class HomelabTags
{
    public const string DeviceId = "device_id";
    public const string DeviceType = "device_type";
    public const string RoomId = "room_id";
}
