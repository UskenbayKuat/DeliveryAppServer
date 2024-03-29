﻿FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/PublicApi/PublicApi.csproj", "PublicApi/"]
COPY ["src/ApplicationCore/ApplicationCore.csproj", "ApplicationCore/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "src/PublicApi/PublicApi.csproj"
COPY . .
WORKDIR "/src/PublicApi"
RUN dotnet build "PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApi.dll"]
