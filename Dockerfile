# =========================
# Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish release build
RUN dotnet publish -c Release -o /app/publish

# =========================
# Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose port (Render will override anyway)
EXPOSE 8080

# Start application
ENTRYPOINT ["dotnet", "GuardifyApi.dll"]