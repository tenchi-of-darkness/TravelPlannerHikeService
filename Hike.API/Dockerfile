FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Hike.API/Hike.API.csproj", "Hike.API/"]
COPY ["Hike.Data/Hike.Data.csproj", "Hike.Data/"]
COPY ["Hike.Domain/Hike.Domain.csproj", "Hike.Domain/"]
COPY ["Hike.UseCases/Hike.UseCases.csproj", "Hike.UseCases/"]
RUN dotnet restore "Hike.API/Hike.API.csproj"
COPY . .
WORKDIR "/src/Hike.API"
RUN dotnet build "Hike.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hike.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hike.API.dll"]