# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TaskManagement.Api/TaskManagement.Api.csproj", "TaskManagement.Api/"]
RUN dotnet restore "TaskManagement.Api/TaskManagement.Api.csproj"

# Copy remaining files and build
COPY . .
WORKDIR "/src/TaskManagement.Api"
RUN dotnet build "TaskManagement.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "TaskManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 12-Factor: Log to stdout (default in .NET Core)
# 12-Factor: Port binding
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "TaskManagement.Api.dll"]
