# Flush Database
dotnet ef database update 0
dotnet ef migrations remove
dotnet ef migrations add Initial
dotnet ef database update

# Make/deploy app
docker build -t singh/stunet-2021 .
heroku container:push -a stunet-2021 web
heroku container:release -a stunet-2021 web
heroku logs -a stunet-2021 --tail


