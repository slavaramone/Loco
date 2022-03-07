FROM mcr.microsoft.com/dotnet/core/runtime:3.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["Tracker.Db/Tracker.Db.csproj", "Tracker/"]
RUN dotnet restore "Tracker.Db/Tracker.Db.csproj"
COPY . .
WORKDIR "/src/Tracker.Db"
RUN dotnet build "Tracker.Db.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracker.Db.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Db.dll"]