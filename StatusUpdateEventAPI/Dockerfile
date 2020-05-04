#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StatusUpdateEventAPI/StatusUpdateEventAPI.csproj", "StatusUpdateEventAPI/"]
COPY ["Associa.Service.BAL/Associa.Service.BAL.csproj", "Associa.Service.BAL/"]
COPY ["Associa.Service.DAL/Associa.Service.DAL.csproj", "Associa.Service.DAL/"]
RUN dotnet restore "StatusUpdateEventAPI/StatusUpdateEventAPI.csproj"
COPY . .
WORKDIR "/src/StatusUpdateEventAPI"
RUN dotnet build "StatusUpdateEventAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatusUpdateEventAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatusUpdateEventAPI.dll"]