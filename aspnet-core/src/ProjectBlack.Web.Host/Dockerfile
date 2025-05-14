FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/ProjectBlack.Web.Host/ProjectBlack.Web.Host.csproj", "src/ProjectBlack.Web.Host/"]
COPY ["src/ProjectBlack.Web.Core/ProjectBlack.Web.Core.csproj", "src/ProjectBlack.Web.Core/"]
COPY ["src/ProjectBlack.Application/ProjectBlack.Application.csproj", "src/ProjectBlack.Application/"]
COPY ["src/ProjectBlack.Core/ProjectBlack.Core.csproj", "src/ProjectBlack.Core/"]
COPY ["src/ProjectBlack.EntityFrameworkCore/ProjectBlack.EntityFrameworkCore.csproj", "src/ProjectBlack.EntityFrameworkCore/"]
WORKDIR "/src/src/ProjectBlack.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ProjectBlack.Web.Host", "src/ProjectBlack.Web.Host"]
COPY ["src/ProjectBlack.Web.Core", "src/ProjectBlack.Web.Core"]
COPY ["src/ProjectBlack.Application", "src/ProjectBlack.Application"]
COPY ["src/ProjectBlack.Core", "src/ProjectBlack.Core"]
COPY ["src/ProjectBlack.EntityFrameworkCore", "src/ProjectBlack.EntityFrameworkCore"]
WORKDIR "/src/src/ProjectBlack.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ProjectBlack.Web.Host.dll"]
