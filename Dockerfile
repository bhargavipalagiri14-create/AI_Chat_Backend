# ✅ BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 🔹 Copy csproj first (for caching)
COPY Backend/API/Api.csproj Backend/API/
WORKDIR /src/Backend/API

RUN dotnet restore

# 🔹 Copy full project
WORKDIR /src
COPY . .

# 🔹 Publish
WORKDIR /src/Backend/API
RUN dotnet publish -c Release -o /app/out

# ✅ RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

# 🔹 Bind to Render port
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# 🔥 IMPORTANT: Use correct DLL name
ENTRYPOINT ["dotnet", "Api.dll"]