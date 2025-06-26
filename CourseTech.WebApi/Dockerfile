FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CourseTech.WebApi/CourseTech.WebApi.csproj", "CourseTech.WebApi/"]
COPY ["CourseTech.Application/CourseTech.Application.csproj", "CourseTech.Application/"]
COPY ["CourseTech.DAL/CourseTech.DAL.csproj", "CourseTech.DAL/"]
COPY ["CourseTech.Domain/CourseTech.Domain.csproj", "CourseTech.Domain/"]
RUN dotnet restore "./CourseTech.WebApi/CourseTech.WebApi.csproj"
COPY . .
WORKDIR "/src/CourseTech.WebApi"
RUN dotnet build "./CourseTech.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CourseTech.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .4
ENTRYPOINT ["dotnet", "CourseTech.WebApi.dll"]