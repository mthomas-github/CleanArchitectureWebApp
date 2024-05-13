FROM nginx:alpine AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ThirdPartyFreight.Web/ThirdPartyFreight.Web.csproj", "src/ThirdPartyFreight.Web/"]
COPY ["src/ThirdPartyFreight.Application/ThirdPartyFreight.Application.csproj", "src/ThirdPartyFreight.Application/"]
COPY ["src/ThirdPartyFreight.Domain/ThirdPartyFreight.Domain.csproj", "src/ThirdPartyFreight.Domain/"]
RUN dotnet nuget add source https://nuget.telerik.com/v3/index.json  --name "MyTelerikFeed" --username "api-key" --password "uWY1ORM+/U6r/A2IZKxl/A==.2WxbnuiD+oiMOxnOzbsjc/5+Q3GQBtPHchGZ1FAj58uleYvKfAMgENfZ1rBaMeLoacxGvIxPz2Hqm63i8H9D+Q==" --store-password-in-clear-text


RUN dotnet restore "src/ThirdPartyFreight.Web/ThirdPartyFreight.Web.csproj"
COPY . .
WORKDIR "/src/src/ThirdPartyFreight.Web"
RUN dotnet build "ThirdPartyFreight.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ThirdPartyFreight.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY src/ThirdPartyFreight.Web/nginx.conf /etc/nginx/nginx.conf
COPY --from=publish /app/publish/wwwroot .