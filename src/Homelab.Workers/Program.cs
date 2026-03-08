using Homelab.Common.Extensions;
using Homelab.InfluxDb.Core.Extensions;
using Homelab.MQTT.Core.Extensions;
using Homelab.Workers.Core.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddHomelabCommon()
    .AddHomelabMqtt()
    .AddHomelabInfluxDb()
    .AddHomelabWorkers();

var host = builder.Build();
host.Run();