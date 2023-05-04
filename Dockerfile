#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BikeStoreWebApi/BikeStoreWebApi.csproj", "BikeStoreWebApi/"]
COPY ["BikeStore.Services/BikeStore.Services.csproj", "BikeStore.Services/"]
COPY ["BikeStoreEF/BikeStore.Core.csproj", "BikeStoreEF/"]
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["BikeStore.DAL/BikeStore.DAL.csproj", "BikeStore.DAL/"]
RUN dotnet restore "BikeStoreWebApi/BikeStoreWebApi.csproj"
COPY . .
WORKDIR "/src/BikeStoreWebApi"
RUN dotnet build "BikeStoreWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BikeStoreWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BikeStoreWebApi.dll"]