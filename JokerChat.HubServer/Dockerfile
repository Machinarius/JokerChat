FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["JokerChat.HubServer/JokerChat.HubServer.csproj", "JokerChat.HubServer/"]
COPY ["JokerChat.Common/JokerChat.Common.csproj", "JokerChat.Common/"]
RUN dotnet restore "JokerChat.HubServer/JokerChat.HubServer.csproj"
COPY . .
WORKDIR "/src/JokerChat.HubServer"
RUN dotnet build "JokerChat.HubServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "JokerChat.HubServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JokerChat.HubServer.dll"]
