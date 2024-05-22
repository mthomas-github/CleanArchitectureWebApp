FROM nginx:1.26.0-alpine-slim AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ThirdPartyFreight.Web/ThirdPartyFreight.Web.csproj", "src/ThirdPartyFreight.Web/"]
COPY ["src/ThirdPartyFreight.Application/ThirdPartyFreight.Application.csproj", "src/ThirdPartyFreight.Application/"]
COPY ["src/ThirdPartyFreight.Domain/ThirdPartyFreight.Domain.csproj", "src/ThirdPartyFreight.Domain/"]
RUN dotnet nuget add source https://nuget.telerik.com/v3/index.json  --name "MyTelerikFeed" --username "api-key" --password "vW6Aedi7d0yc9D+ZWyPuPQ==.QNy4q9ceRlp/GeDMKhuGTPAhKx9W/qfP517HVJLt47RdNzHJ5VTJ6WrbcOU1dAfR00w2eir5dIstzRCQe9D4hQ==" --store-password-in-clear-text


RUN dotnet restore "src/ThirdPartyFreight.Web/ThirdPartyFreight.Web.csproj"
COPY . .
WORKDIR "/src/src/ThirdPartyFreight.Web"
RUN dotnet build "ThirdPartyFreight.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Released
RUN dotnet publish "ThirdPartyFreight.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .