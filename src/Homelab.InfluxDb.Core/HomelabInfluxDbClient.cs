using Homelab.InfluxDb.Abstraction;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;

namespace Homelab.InfluxDb.Core;

internal class HomelabInfluxDbClient(
    IOptions<HomelabInfluxDbOptions> options) 
    : IHomelabInfluxDbClient
{
    public Task SaveDataAsync(
        HomelabInfluxDbSaveOptions saveOptions,
        CancellationToken cancellationToken = default)
    {
        using var client = new InfluxDBClient(
            options.Value.Host,
            options.Value.Token);

        using var writeApi = client.GetWriteApi();

        var point = PointData.Measurement(saveOptions.Measurement)
            .Timestamp(saveOptions.Timestamp, WritePrecision.Ns);

        foreach (var (key, value) in saveOptions.Fields)
        {
            point = point.Field(key, value.ToString());
        }

        if (saveOptions.Tags is not null)
        {
            foreach (var (key, value) in saveOptions.Tags)
            {
                point = point.Tag(key, value);
            }
        }
        
        writeApi.WritePoint(point, saveOptions.Bucket, options.Value.Organization);
        
        return Task.CompletedTask;
    }
}
