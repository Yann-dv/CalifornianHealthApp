FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

ENV ASPNETCORE_SCOPE="docker"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CalifornianHealthCalendarApi/CalifornianHealthCalendarApi.csproj", "./"]
RUN dotnet restore "CalifornianHealthCalendarApi.csproj"

COPY . .
WORKDIR "/src/CalifornianHealthCalendarApi"
RUN dotnet build "CalifornianHealthCalendarApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalifornianHealthCalendarApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CalifornianHealthCalendarApi.dll"]