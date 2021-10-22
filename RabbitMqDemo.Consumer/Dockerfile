# Build stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY "RabbitMqDemo.Consumer.csproj" .
RUN dotnet restore "RabbitMqDemo.Consumer.csproj"
COPY . .
RUN dotnet build "RabbitMqDemo.Consumer.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish "RabbitMqDemo.Consumer.csproj" -c Release -o /src/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "RabbitMqDemo.Consumer.dll"]