FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Plan.API/Plan.API.csproj", "Plan.API/"]
RUN dotnet restore "Plan.API/Plan.API.csproj"
COPY . .
WORKDIR "/src/Plan.API"
RUN dotnet build "Plan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plan.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plan.API.dll"]
