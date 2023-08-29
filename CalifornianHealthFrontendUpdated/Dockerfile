FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CalifornianHealthFrontendUpdated/CalifornianHealthFrontendUpdated.csproj", "./"]
RUN dotnet restore "CalifornianHealthFrontendUpdated.csproj"
COPY . .
WORKDIR "/src/CalifornianHealthFrontendUpdated"
RUN dotnet build "CalifornianHealthFrontendUpdated.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalifornianHealthFrontendUpdated.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CalifornianHealthFrontendUpdated.dll"]