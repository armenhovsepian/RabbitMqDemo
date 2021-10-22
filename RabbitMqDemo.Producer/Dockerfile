# Build stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY "RabbitMqDemo.Producer.csproj" .
RUN dotnet restore "RabbitMqDemo.Producer.csproj"
COPY . .
RUN dotnet build "RabbitMqDemo.Producer.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish "RabbitMqDemo.Producer.csproj" -c Release -o /src/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "RabbitMqDemo.Producer.dll"]