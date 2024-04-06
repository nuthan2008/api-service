# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the ASP.NET Core SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy the .csproj files and restore dependencies
COPY ["api.service/api.service.csproj","api.service/"]
COPY ["BusinessProvider/BusinessProvider.csproj", "BusinessProvider/"]
COPY ["DataProvider/DataProvider.csproj", "DataProvider/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "api.service/api.service.csproj"
# Copy the entire solution and build
COPY . .
WORKDIR "/src/api.service"
RUN dotnet build "api.service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.service.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM runtime AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.service.dll"]