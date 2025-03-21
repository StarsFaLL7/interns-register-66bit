FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /blazorApp
EXPOSE 7200
EXPOSE 7201

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClientApp/ClientApp.csproj", "ClientApp/"]
RUN dotnet restore "ClientApp/ClientApp.csproj"
COPY . .
WORKDIR "/src/ClientApp"
RUN dotnet build "ClientApp.csproj" -c $BUILD_CONFIGURATION -o /blazorApp/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClientApp.csproj" -c $BUILD_CONFIGURATION -o /blazorApp/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /blazorApp
COPY --from=publish /blazorApp/publish .

ENTRYPOINT ["dotnet", "ClientApp.dll"]