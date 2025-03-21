FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /apiServer
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InternRegister/InternRegister.csproj", "InternRegister/"]
RUN dotnet restore "InternRegister/InternRegister.csproj"
COPY . .
WORKDIR "/src/InternRegister"
RUN dotnet build "InternRegister.csproj" -c $BUILD_CONFIGURATION -o /apiServer/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InternRegister.csproj" -c $BUILD_CONFIGURATION -o /apiServer/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /apiServer
COPY --from=publish /apiServer/publish .

ENTRYPOINT ["dotnet", "InternRegister.dll"]