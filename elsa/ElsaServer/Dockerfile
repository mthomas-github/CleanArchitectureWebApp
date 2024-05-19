FROM mcr.microsoft.com/dotnet/aspnet:8.0-stable-slim AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-stable-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["elsa/ElsaServer/ElsaServer.csproj", "elsa/ElsaServer/"]
RUN dotnet restore "elsa/ElsaServer/ElsaServer.csproj"
COPY . .
WORKDIR "/src/elsa/ElsaServer"
RUN dotnet build "ElsaServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ElsaServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
USER root
# Install Python 3.11
RUN apt-get update && apt-get install -y --no-install-recommends \
    python3.11 \
    python3.11-dev \
    libpython3.11 \
    python3-pip && \
    rm -rf /var/lib/apt/lists/* \
    
RUN sudo update-ca-certificates

# Set PYTHONNET_PYDLL environment variable
ENV PYTHONNET_PYDLL /usr/lib/aarch64-linux-gnu/libpython3.11.so
USER $APP_UID
ENTRYPOINT ["dotnet", "ElsaServer.dll"]
