# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the .csproj file from the server1 directory
COPY ./Server1.csproj ./Server1.csproj

# Restore dependencies
RUN dotnet restore ./Server1.csproj

# Copy the rest of the application files from the server1 directory
COPY . ./

# Publish the application
RUN dotnet publish ./Server1.csproj -c Release -o out

# Use the official .NET ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out . 

# Define the entry point for the application
ENTRYPOINT ["dotnet", "Server1.dll"]
