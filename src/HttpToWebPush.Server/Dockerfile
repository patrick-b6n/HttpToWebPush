FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/HttpToWebPush.Server/HttpToWebPush.Server.csproj", "HttpToWebPush.Server/"]
RUN dotnet restore "src/HttpToWebPush.Server/HttpToWebPush.Server.csproj"
COPY . .
WORKDIR "/src/HttpToWebPush.Server"
RUN dotnet build "HttpToWebPush.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HttpToWebPush.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HttpToWebPush.Server.dll"]
