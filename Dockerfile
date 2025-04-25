# Use the .NET 9.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and all project files (including .csproj) into the container
COPY VIA-Tabloid.sln ./
COPY EFC/EFC.csproj ./EFC/
COPY VIATabloidAPI/VIATabloidAPI.csproj ./VIATabloidAPI/

# Restore dependencies for all projects in the solution
RUN dotnet restore VIA-Tabloid.sln

# Copy the rest of the application code
COPY . ./

# Publish the app to the /out directory
RUN dotnet publish VIATabloidAPI/VIATabloidAPI.csproj -c Release -o /out

# Use the .NET 9.0 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /out .

# Expose the port for the app to run on
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "VIATabloidAPI.dll"]
