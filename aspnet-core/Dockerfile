# Etapa base de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar toda la solución
COPY . .

# Restaurar dependencias apuntando al .sln
WORKDIR /src/aspnet-core
RUN dotnet restore ProjectBlack.sln

# Publicar apuntando al .sln
RUN dotnet publish ProjectBlack.sln -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Ejecutar migración antes de iniciar si se pasa --migrate
ENTRYPOINT ["sh", "-c", "dotnet ProjectBlack.Web.Host.dll --migrate || true && dotnet ProjectBlack.Web.Host.dll"]
