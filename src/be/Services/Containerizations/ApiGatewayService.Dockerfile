FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Services/Fakebook.sln", "./be/Services/"]

COPY ["./be/Services/Fakebook.ApiGateway/Fakebook.ApiGateway.csproj", "./be/Services/Fakebook.ApiGateway/"]
RUN dotnet restore "./be/Services/Fakebook.ApiGateway/Fakebook.ApiGateway.csproj"

COPY ["./be/Services/Fakebook.ApiGateway/", "./be/Services/Fakebook.ApiGateway/"]
WORKDIR /src/be/Services/Fakebook.ApiGateway
RUN dotnet build "./Fakebook.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.ApiGateway.dll"]