# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./QuizSystem/QuizSystem.csproj", "."]
RUN dotnet restore "./QuizSystem.csproj"
COPY ./QuizSystem .
WORKDIR "/src/."
RUN dotnet build "./QuizSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM node:lts-alpine AS build-frontend
WORKDIR /app
COPY ./QuizSystemReact/package.json ./QuizSystemReact/package-lock.json /app
RUN npm install
COPY ./QuizSystemReact /app
RUN npm run build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuizSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY --from=build-frontend /app/dist/ /app/publish/wwwroot

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizSystem.dll"]

