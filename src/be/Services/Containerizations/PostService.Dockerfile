FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./be/Services/Fakebook.sln", "./be/Services/"]

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/Fakebook.DataAccessLayer.csproj", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Commons/Fakebook.SynchronousModel/Fakebook.SynchronousModel.csproj", "./be/Services/Commons/Fakebook.SynchronousModel/"]
COPY ["./be/Services/Commons/Fakebook.MessageQueueHandler/Fakebook.MessageQueueHandler.csproj", "./be/Services/Commons/Fakebook.MessageQueueHandler/"]
COPY ["./be/Services/Fakebook.PostService/Fakebook.PostService.csproj", "./be/Services/Fakebook.PostService/"]
RUN dotnet restore "./be/Services/Fakebook.PostService/Fakebook.PostService.csproj"

COPY ["./be/Services/Commons/Fakebook.DataAccessLayer/", "./be/Services/Commons/Fakebook.DataAccessLayer/"]
COPY ["./be/Services/Commons/Fakebook.SynchronousModel/", "./be/Services/Commons/Fakebook.SynchronousModel/"]
COPY ["./be/Services/Commons/Fakebook.MessageQueueHandler/", "./be/Services/Commons/Fakebook.MessageQueueHandler/"]
COPY ["./be/Services/Fakebook.PostService/", "./be/Services/Fakebook.PostService/"]
WORKDIR /src/be/Services/Fakebook.PostService
RUN dotnet build "./Fakebook.PostService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fakebook.PostService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.PostService.dll"]
