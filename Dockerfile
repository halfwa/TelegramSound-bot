FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["/TelegramSound-bot/TelegramSound.csproj", "./"]
RUN dotnet restore "TelegramSound.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TelegramSound.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramSound.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramSound.dll"]  