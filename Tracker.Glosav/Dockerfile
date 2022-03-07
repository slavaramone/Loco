FROM mcr.microsoft.com/dotnet/core/runtime:3.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["Tracker.Glosav/Tracker.Glosav.csproj", "Tracker.Glosav/"]
RUN dotnet restore "Tracker.Glosav/Tracker.Glosav.csproj"
COPY . .
WORKDIR "/src/Tracker.Glosav"
RUN dotnet build "Tracker.Glosav.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracker.Glosav.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Glosav.dll"]