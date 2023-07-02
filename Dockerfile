# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# ENV ASPNETCORE_ENVIRONMENT=Development
# EXPOSE 80

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /app
# COPY . .

# WORKDIR "/app/src/PublicApi"
# RUN dotnet restore

# RUN dotnet build "./PublicApi.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "./PublicApi.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "PublicApi.dll"]

# Use the appropriate base image for your .NET version and Linux distribution
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# RUN dotnet restore AML.Dictionary.API/AML.Dictionary.API.csproj

# Copy the entire project and build the application
COPY . .
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set the entry point for your application
ENTRYPOINT ["dotnet", "PublicApi.dll"]