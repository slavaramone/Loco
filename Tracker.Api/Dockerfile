#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Tracker.Api/Tracker.Api.csproj", "Tracker.Api/"]
RUN dotnet restore "Tracker.Api/Tracker.Api.csproj"
COPY . .
WORKDIR "/src/Tracker.Api"
RUN dotnet build "Tracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Api.dll"]