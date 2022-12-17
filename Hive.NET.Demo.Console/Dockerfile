FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Hive.NET.Demo.Console/Hive.NET.Demo.Console.csproj", "Hive.NET.Demo.Console/"]
RUN dotnet restore "Hive.NET.Demo.Console/Hive.NET.Demo.Console.csproj"
COPY . .
WORKDIR "/src/Hive.NET.Demo.Console"
RUN dotnet build "Hive.NET.Demo.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hive.NET.Demo.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hive.NET.Demo.Console.dll"]
