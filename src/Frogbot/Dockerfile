# Development Stage - for coding and testing
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development
WORKDIR /app

# Install git for dev container
RUN apt-get update && apt-get install -y git && rm -rf /var/lib/apt/lists/*

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application
COPY . ./

# For dev containers: keep running, let dev container handle execution
CMD ["sleep", "infinity"]

# Build the app in a separate stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY *.csproj ./Frogbot/
RUN dotnet restore ./Frogbot/Frogbot.csproj
COPY . ./Frogbot/
RUN dotnet publish ./Frogbot/Frogbot.csproj -c Release -o /app

# Final production image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Frogbot.dll"]