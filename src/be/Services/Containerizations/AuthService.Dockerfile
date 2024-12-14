FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Services/Fakebook.sln", "./be/Services/"]

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/Fakebook.DataAccessLayer.csproj", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Fakebook.AuthService/Fakebook.AuthService.csproj", "./be/Services/Fakebook.AuthService/"]
RUN dotnet restore "./be/Services/Fakebook.AuthService/Fakebook.AuthService.csproj"

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Fakebook.AuthService/", "./be/Services/Fakebook.AuthService/"]
WORKDIR /src/be/Services/Fakebook.AuthService
RUN dotnet build "./Fakebook.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.AuthService.dll"]
