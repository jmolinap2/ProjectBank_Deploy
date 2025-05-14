FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./aspnet-core .

RUN dotnet restore ProjectBlack.sln
RUN dotnet publish ProjectBlack.sln -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Ejecuta migraciones si se pasa --migrate
ENTRYPOINT ["sh", "-c", "dotnet ProjectBlack.Web.Host.dll --migrate || true && dotnet ProjectBlack.Web.Host.dll"]
