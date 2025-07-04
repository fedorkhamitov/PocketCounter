FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PocketCounter.Api/PocketCounter.Api.csproj", "src/PocketCounter.Api/"]
COPY ["src/PocketCounter.Application/PocketCounter.Application.csproj", "src/PocketCounter.Application/"]
COPY ["src/PocketCounter.Domain/PocketCounter.Domain.csproj", "src/PocketCounter.Domain/"]
COPY ["src/PocketCounter.Infrastructure.Authentication/PocketCounter.Infrastructure.Authentication.csproj", "src/PocketCounter.Infrastructure.Authentication/"]
COPY ["src/PocketCounter.Infrastructure/PocketCounter.Infrastructure.csproj", "src/PocketCounter.Infrastructure/"]
RUN dotnet restore "src/PocketCounter.Api/PocketCounter.Api.csproj"
COPY . .
WORKDIR "/src/src/PocketCounter.Api"
RUN dotnet build "PocketCounter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PocketCounter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PocketCounter.Api.dll"]
