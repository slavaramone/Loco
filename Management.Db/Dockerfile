#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Management.Db/Management.Db.csproj", "Management.Db/"]
RUN dotnet restore "Management.Db/Management.Db.csproj"
COPY . .
WORKDIR "/src/Management.Db"
RUN dotnet build "Management.Db.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Management.Db.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.Db.dll"]