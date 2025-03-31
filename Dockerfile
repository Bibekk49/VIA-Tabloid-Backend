# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["VIA-Tabloid.sln", "./"]
COPY ["VIATabloidAPI/VIATabloidAPI.csproj", "VIATabloidAPI/"]
COPY ["EFC/EFC.csproj", "EFC/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application source code
COPY . .

# Build the application
WORKDIR /src/VIATabloidAPI
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use a runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create a non-root user and set permissions
RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app
USER appuser

# Copy the built application
COPY --from=build /app/publish .

# Expose the application port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "VIATabloidAPI.dll"]
