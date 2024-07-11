FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
# Optional: Set ASP.NET Core environment to Production
ENV ASPNETCORE_ENVIRONMENT Production
ARG APP_PORT=5001
ENV ASPNETCORE_URLS http://*:$APP_PORT
EXPOSE $APP_PORT
ENTRYPOINT ["dotnet", "nginx-deployer.dll"]