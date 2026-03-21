FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Homelab.slnx ./

# Common
COPY src/Homelab.Common/Homelab.Common.csproj ./src/Homelab.Common/
# Shelly
COPY src/Homelab.Shelly.Abstraction/Homelab.Shelly.Abstraction.csproj ./src/Homelab.Shelly.Abstraction/
# MQTT
COPY src/Homelab.MQTT.Abstraction/Homelab.MQTT.Abstraction.csproj ./src/Homelab.MQTT.Abstraction/
COPY src/Homelab.MQTT.Core/Homelab.MQTT.Core.csproj ./src/Homelab.MQTT.Core/
# InfluxDb
COPY src/Homelab.InfluxDb.Abstraction/Homelab.InfluxDb.Abstraction.csproj ./src/Homelab.InfluxDb.Abstraction/
COPY src/Homelab.InfluxDb.Core/Homelab.InfluxDb.Core.csproj ./src/Homelab.InfluxDb.Core/
# Workers
COPY src/Homelab.Workers.Abstraction/Homelab.Workers.Abstraction.csproj ./src/Homelab.Workers.Abstraction/
COPY src/Homelab.Workers.Core/Homelab.Workers.Core.csproj ./src/Homelab.Workers.Core/
COPY src/Homelab.Workers/Homelab.Workers.csproj ./src/Homelab.Workers/
# tests
COPY tests/Homelab.Shelly.Abstraction.UnitTests/Homelab.Shelly.Abstraction.UnitTests.csproj ./tests/Homelab.Shelly.Abstraction.UnitTests/

RUN dotnet restore

COPY . ./

FROM build AS publish

RUN dotnet publish /app/src/Homelab.Workers/Homelab.Workers.csproj \
    --no-restore \
    -c Release \
    -o /app/out \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/out ./

ENTRYPOINT ["dotnet", "Homelab.Workers.dll"]