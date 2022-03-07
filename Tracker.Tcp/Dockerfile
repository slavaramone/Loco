FROM mcr.microsoft.com/dotnet/core/runtime:3.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["Tracker.Tcp/Tracker.Tcp.csproj", "Tracker.Tcp/"]
RUN dotnet restore "Tracker.Tcp/Tracker.Tcp.csproj"
COPY . .
WORKDIR "/src/Tracker.Tcp"
RUN dotnet build "Tracker.Tcp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracker.Tcp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Tcp.dll"]