#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-stable-slim AS base
# Update package lists and install curl
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-stable-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ThirdPartyFreight.Api/ThirdPartyFreight.Api.csproj", "src/ThirdPartyFreight.Api/"]
RUN dotnet restore "./src/ThirdPartyFreight.Api/ThirdPartyFreight.Api.csproj"
COPY . .
WORKDIR "/src/src/ThirdPartyFreight.Api"
RUN dotnet build "./ThirdPartyFreight.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ThirdPartyFreight.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThirdPartyFreight.Api.dll"]