FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
# Add healthcheck
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CategoryManagement.API/CategoryManagement.API.csproj", "CategoryManagement.API/"]
RUN dotnet restore "CategoryManagement.API/CategoryManagement.API.csproj"
COPY . .
WORKDIR "/src/CategoryManagement.API"
RUN dotnet build "CategoryManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CategoryManagement.API.dll"]
