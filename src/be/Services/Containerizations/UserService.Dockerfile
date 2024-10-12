FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Fakebook.sln", "./"]

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/Fakebook.DataAccessLayer.csproj", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Fakebook.UserService/Fakebook.UserService.csproj", "./be/Services/Fakebook.UserService/"]
RUN dotnet restore "./be/Services/Fakebook.UserService/Fakebook.UserService.csproj"

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Fakebook.UserService/", "./be/Services/Fakebook.UserService/"]
WORKDIR /src/be/Services/Fakebook.UserService
RUN dotnet build "./Fakebook.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.UserService.dll"]
