FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["JokerChat.Endpoint/JokerChat.Endpoint.csproj", "JokerChat.Endpoint/"]
COPY ["JokerChat.Common/JokerChat.Common.csproj", "JokerChat.Common/"]
RUN dotnet restore "JokerChat.Endpoint/JokerChat.Endpoint.csproj"
COPY . .
WORKDIR "/src/JokerChat.Endpoint"
RUN dotnet build "JokerChat.Endpoint.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "JokerChat.Endpoint.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JokerChat.Endpoint.dll"]