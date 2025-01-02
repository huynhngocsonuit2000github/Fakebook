FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Services/Fakebook.sln", "./be/Services/"]

COPY ["./be/Services/Fakebook.AIO/Fakebook.AIO.csproj", "./be/Services/Fakebook.AIO/"]
RUN dotnet restore "./be/Services/Fakebook.AIO/Fakebook.AIO.csproj"

COPY ["./be/Services/Fakebook.AIO/", "./be/Services/Fakebook.AIO/"]
WORKDIR /src/be/Services/Fakebook.AIO
RUN dotnet build "./Fakebook.AIO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.AIO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.AIO.dll"]
