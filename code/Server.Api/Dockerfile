FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
RUN mkdir  -p  Services/Email/Templates/
COPY ./Services/Email/Templates/ /app/Services/Email/Templates/

RUN mkdir  -p  Downloads
COPY ./Downloads /app/Downloads/

EXPOSE 80
EXPOSE 5000
EXPOSE 5001
COPY --from=build-env /app/out .
# ENTRYPOINT [ "dotnet", "Server.Api.dll"]
# Use the following instead for Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Server.Api.dll
