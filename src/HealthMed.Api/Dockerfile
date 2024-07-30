FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HealthMed.Api/HealthMed.Api.csproj", "src/HealthMed.Api/"]
COPY ["src/HealthMed.Application/HealthMed.Application.csproj", "src/HealthMed.Application/"]
COPY ["src/HealthMed.Domain/HealthMed.Domain.csproj", "src/HealthMed.Domain/"]
COPY ["src/HealthMed.CrossCutting/HealthMed.CrossCutting.csproj", "src/HealthMed.CrossCutting/"]
COPY ["src/HealthMed.Infra/HealthMed.Infra.csproj", "src/HealthMed.Infra/"]

RUN dotnet restore "./src/HealthMed.Api/HealthMed.Api.csproj"
COPY . .
WORKDIR "/src/src/HealthMed.Api"
RUN dotnet build "./HealthMed.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthMed.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.Api.dll"]