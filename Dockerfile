# Use the official .NET Framework image as the base
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build-env

# Set the working directory
WORKDIR /app

# Copy the .NET app files to the container
COPY ./Binary/*.csproj ./
RUN dotnet restore

# Copy the rest of the app files
COPY ./Binary ./

# Build the app
RUN dotnet publish -c Release -o out

# Create the final runtime image
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
WORKDIR /app
COPY --from=build-env /app/out ./

# Run the app
CMD ["Binary.exe"]  # Replace with your actual executable name
