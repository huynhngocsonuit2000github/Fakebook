FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Services/Fakebook.sln", "./be/Services/"]

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/Fakebook.DataAccessLayer.csproj", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Commons/Fakebook.SynchronousModel/Fakebook.SynchronousModel.csproj", "./be/Services/Commons/Fakebook.SynchronousModel/"]
COPY ["./be/Services/Fakebook.IdPService/Fakebook.IdPService.csproj", "./be/Services/Fakebook.IdPService/"]
RUN dotnet restore "./be/Services/Fakebook.IdPService/Fakebook.IdPService.csproj"

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Commons/Fakebook.SynchronousModel/", "./be/Services/Commons/Fakebook.SynchronousModel/"]
COPY ["./be/Services/Fakebook.IdPService/", "./be/Services/Fakebook.IdPService/"]
WORKDIR /src/be/Services/Fakebook.IdPService
RUN dotnet build "./Fakebook.IdPService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.IdPService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.IdPService.dll"]