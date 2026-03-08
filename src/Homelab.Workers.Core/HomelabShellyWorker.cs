using System.Text.Json;
using Homelab.InfluxDb.Abstraction;
using Homelab.MQTT.Abstraction;
using Homelab.Shelly.Abstraction.Constants;
using Homelab.Shelly.Abstraction.Messages;
using Homelab.Workers.Abstraction;
using Homelab.Workers.Core.Constants;
using Homelab.Workers.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homelab.Workers.Core;

public class HomelabShellyWorker(
    IHomelabInfluxDbClient influxDbClient,
    IHomelabMqttClientFactory mqttClientFactory,
    TimeProvider timeProvider,
    IOptions<HomelabShellyWorkerOptions> options,
    ILogger<HomelabShellyWorker> logger) 
    : HomelabMqttWorker<ShellyMessage<JsonElement>, HomelabShellyWorkerOptions>(
        mqttClientFactory,
        timeProvider,
        options.Value,
        logger)
{
    protected override async Task OnMessageReceivedAsync(ShellyMessage<JsonElement> message)
    {
        if (!message.Method.Equals(ShellyMethods.NotifyFullStatus, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("{Class}: Received message with method: {Method}. Skipping.",
                nameof(HomelabShellyWorker),
                message.Method);
            return;
        }

        var content = ShellyMessage<ShellyNotifyFullStatusParams>.FromJsonPayload(
            message);
                
        var data = new HomelabMeasurementDto
        {
            BatteryPercent = content.Params.DevicePowerZero.Battery.Percent,
            BatteryVoltage = content.Params.DevicePowerZero.Battery.V,
            TemperatureC = content.Params.TemperatureZero.Tc,
            RelativeHumidity = content.Params.HumidityZero.Rh,
            WifiRssi = content.Params.Wifi.Rssi,
            Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)content.Params.Ts)
        };
        
        await SaveDataAsync(data);
    }

    private async Task SaveDataAsync(HomelabMeasurementDto data)
    {
        var tags = new Dictionary<string, string>
        {
            [HomelabTags.DeviceId] = options.Value.DeviceId,
            [HomelabTags.DeviceType] = options.Value.DeviceType,
            [HomelabTags.RoomId] = options.Value.RoomId,
        };
        
        await influxDbClient.SaveDataAsync(new HomelabInfluxDbSaveOptions
        {
            Bucket = HomelabSensors.Bucket,
            Fields = new Dictionary<string, object>
            {
                [HomelabSensors.Measurements.Environment.Fields.TemperatureC] = data.TemperatureC,
                [HomelabSensors.Measurements.Environment.Fields.RelativeHumidity] = data.RelativeHumidity,
            },
            Measurement = HomelabSensors.Measurements.Environment.Name,
            Tags = tags,
            Timestamp = data.Timestamp
        });
        
        await SaveTelemetryAsync(data, tags);
        
        logger.LogInformation("{Class}: Data saved: {Data}",
            nameof(HomelabShellyWorker),
            data);
    }

    private async Task SaveTelemetryAsync(HomelabMeasurementDto data, Dictionary<string, string> tags)
    {
        if (!options.Value.IsTelemetryEnabled)
        {
            return;
        }

        await influxDbClient.SaveDataAsync(new HomelabInfluxDbSaveOptions
        {
            Bucket = HomelabSensors.Bucket,
            Fields = new Dictionary<string, object>
            {
                [HomelabSensors.Measurements.Telemetry.Fields.BatteryPercent] = data.BatteryPercent,
                [HomelabSensors.Measurements.Telemetry.Fields.BatteryVoltage] = data.BatteryVoltage,
                [HomelabSensors.Measurements.Telemetry.Fields.WifiRssi] = data.WifiRssi 
            },
            Measurement = HomelabSensors.Measurements.Telemetry.Name,
            Tags = tags,
            Timestamp = data.Timestamp
        });
    }
}