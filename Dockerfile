#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SoxMon2/SoxMon2.csproj", "SoxMon2/"]
RUN dotnet restore "SoxMon2/SoxMon2.csproj"
COPY . .
WORKDIR "/src/SoxMon2"
RUN dotnet build "SoxMon2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoxMon2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoxMon2.dll"]