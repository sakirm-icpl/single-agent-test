#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Runtime image
FROM cblmariner/mariner-dotnet-runtime-deps:2.0 AS runtime
WORKDIR /app  

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy csproj and restore as distinct layers
# Copy csproj files and restore any dependencies (assumes WazuhCommon is in a directory at the same level as WazuhApi)
COPY ["WazuhCommon/WazuhCommon.csproj", "WazuhCommon/"]
COPY ["WazuhApi/WazuhApi.csproj", "WazuhApi/"]

# Restore dependencies for both projects
RUN dotnet restore "WazuhCommon/WazuhCommon.csproj"
RUN dotnet restore "WazuhApi/WazuhApi.csproj"

# Copy everything else and build
COPY . .
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Set the working directory and publish the application
WORKDIR "/src/WazuhApi"

# Please see https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props for a list of publish properties
RUN dotnet publish "./WazuhApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:PublishAot=true \
    /p:SelfContained=true

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./WazuhApi"]