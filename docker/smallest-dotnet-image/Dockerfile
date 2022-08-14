FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.14 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 as publish
WORKDIR /src

COPY . .
RUN dotnet publish "smallest-dotnet-image.csproj" -f net6.0 -c Release -o /out -r alpine-x64 --sc true /p:PublishTrimmed=true /p:TrimMode=Link /p:PublishSingleFile=true

FROM base AS final
WORKDIR /app
COPY --from=publish /out .
RUN ls
ENTRYPOINT [ "./smallest-dotnet-image" ]