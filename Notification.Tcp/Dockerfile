FROM mcr.microsoft.com/dotnet/core/runtime:3.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["Notification.Tcp/Notification.Tcp.csproj", "Notification.Tcp/"]
RUN dotnet restore "Notification.Tcp/Notification.Tcp.csproj"
COPY . .
WORKDIR "/src/Notification.Tcp"
RUN dotnet build "Notification.Tcp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.Tcp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.Tcp.dll"]