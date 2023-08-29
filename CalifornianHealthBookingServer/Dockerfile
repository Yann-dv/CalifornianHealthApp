FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy and restore NuGet packages
COPY ["CalifornianHealthBookingServer/CalifornianHealthBookingServer.csproj", "./"]
RUN dotnet restore "CalifornianHealthBookingServer.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
WORKDIR "/src/CalifornianHealthBookingServer"
RUN dotnet build "CalifornianHealthBookingServer.csproj" -c Release -o /app/build

# Build the final image
FROM base AS final
WORKDIR /app

COPY --from=build /app/build .

ENTRYPOINT ["dotnet", "CalifornianHealthBookingServer.dll"]
