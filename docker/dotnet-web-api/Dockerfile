FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.16 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.16 AS build

WORKDIR /src

COPY ./dotnet-web-api.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish "dotnet-web-api.csproj" -c Release -o /publish

FROM base AS runtime

COPY --from=build /publish .

ENTRYPOINT ["dotnet","dotnet-web-api.dll"]