FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["neo4j-first-run/neo4j-first-run.csproj", "neo4j-first-run/"]
RUN dotnet restore "neo4j-first-run/neo4j-first-run.csproj"
COPY . .
WORKDIR "/src/neo4j-first-run"
RUN dotnet build "neo4j-first-run.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "neo4j-first-run.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "neo4j-first-run.dll"]
