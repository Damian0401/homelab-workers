using System.Text.Json;

using Homelab.Shelly.Abstraction.Messages;

namespace Homelab.Shelly.Abstraction.UnitTests.Tests.Messages;

public class ShellyMessageTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };
    
    [Fact]
    public void FromJsonPayload_WhenDeserializedNotifyEvent_ShouldHaveCorrectValues()
    {
        // Arrange
        var content = File.ReadAllText("./Data/NotifyEvent.json");
        var expected = new ShellyMessage<ShellyNotifyEventParams>
        {
            Src = "shellyhtg3-<anonymized>",
            Dst = "shellyhtg3-<anonymized>/events",
            Method = "NotifyEvent",
            Params = new()
            {
                Ts = 1771974788.40,
                Events =
                [
                    new()
                    {
                        Component = "sys",
                        Event = "sleep",
                        Ts = 1771974788.40
                    }
                ]
            }
        };

        
        // Act
        var message = JsonSerializer.Deserialize<ShellyMessage<JsonElement>>(
            content,
            _options);
        var typedMessage = ShellyMessage<ShellyNotifyEventParams>.FromJsonPayload(message!);
        
        // Assert
        Assert.Equivalent(expected, typedMessage);
    }
    
    [Fact]
    public void FromJsonPayload_WhenDeserializedNotifyStatus_ShouldHaveCorrectValues()
    {
        // Arrange
        var content = File.ReadAllText("./Data/NotifyStatus.json");
        var expected = new ShellyMessage<ShellyNotifyStatusParams>
        {
            Src = "shellyhtg3-<anonymized>",
            Dst = "shellyhtg3-<anonymized>/events",
            Method = "NotifyStatus",
            Params = new()
            {
                Ts = 1771974786.30,
                Mqtt = new()
                {
                    Connected = true
                }
            }
        };

        
        // Act
        var message = JsonSerializer.Deserialize<ShellyMessage<JsonElement>>(
            content,
            _options);
        var typedMessage = ShellyMessage<ShellyNotifyStatusParams>.FromJsonPayload(message!);
        
        // Assert
        Assert.Equivalent(expected, typedMessage);
    }

    [Fact]
    public void FromJsonPayload_WhenDeserializedNotifyFullStatus_ShouldHaveCorrectValues()
    {
        // Arrange
        var content = File.ReadAllText("./Data/NotifyFullStatus.json");
        var expected = new ShellyMessage<ShellyNotifyFullStatusParams>
        {
            Src = "shellyhtg3-<anonymized>",
            Dst = "shellyhtg3-<anonymized>/events",
            Method = "NotifyFullStatus",
            Params = new()
            {
                Ts = 1771974725.38,
                Ble = new(),
                Cloud = new() { Connected = false },
                DevicePowerZero = new()
                {
                    Id = 0,
                    Battery = new() { V = 5.90, Percent = 95 },
                    External = new() { Present = false }
                },
                HtUi = new(),
                HumidityZero = new() { Id = 0, Rh = 59.0 },
                Mqtt = new() { Connected = true },
                Sys = new()
                {
                    Mac = "<anonymized>",
                    RestartRequired = false,
                    Time = null,
                    Unixtime = null,
                    Uptime = 5,
                    RamSize = 255592,
                    RamFree = 115400,
                    FsSize = 1048576,
                    FsFree = 757760,
                    CfgRev = 10,
                    KvsRev = 0,
                    WebhookRev = 0,
                    AvailableUpdates = new(),
                    WakeupReason = new()
                    {
                        Boot = "deepsleep_wake",
                        Cause = "status_update"
                    },
                    WakeupPeriod = 7200,
                    ResetReason = 8
                },
                TemperatureZero = new()
                {
                    Id = 0, 
                    Tc = 29.3,
                    Tf = 84.7
                },
                Wifi = new()
                {
                    StaIp = "<anonymized>",
                    Status = "got ip",
                    Ssid = "<anonymized>",
                    Rssi = -80
                },
                Ws = new()
                {
                    Connected = false
                }
            }
        };


        
        // Act
        var message = JsonSerializer.Deserialize<ShellyMessage<JsonElement>>(
            content,
            _options);
        var typedMessage = ShellyMessage<ShellyNotifyFullStatusParams>.FromJsonPayload(message!);
        
        // Assert
        Assert.Equivalent(expected, typedMessage);
    }
}