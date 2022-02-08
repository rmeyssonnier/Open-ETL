# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY PampaSoft.Data.Etl.sln ./
COPY PampaSoft.Data.Etl.Api/*.csproj ./PampaSoft.Data.Etl.Api/
COPY PampaSoft.Data.Etl.Engine/*.csproj ./PampaSoft.Data.Etl.Engine/
COPY PampaSoft.Data.Etl.Tests/*.csproj ./PampaSoft.Data.Etl.Tests/

RUN dotnet restore

COPY PampaSoft.Data.Etl.Api ./PampaSoft.Data.Etl.Api/
COPY PampaSoft.Data.Etl.Engine ./PampaSoft.Data.Etl.Engine/
COPY PampaSoft.Data.Etl.Tests ./PampaSoft.Data.Etl.Tests/

WORKDIR /app/PampaSoft.Data.Etl.Api
RUN dotnet publish -c Release -o out

## Copy everything else and build
#COPY ../engine/examples ./
#RUN dotnet publish -c Release -o out
#
## Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/PampaSoft.Data.Etl.Api/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "PampaSoft.Data.Etl.Api.dll"]