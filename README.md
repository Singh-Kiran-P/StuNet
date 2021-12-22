# **StuNet**

# Install
Make sure you have docker, docker compose, java v8 and nodejs installed
```
cd code
npm run upgrade
```

# Start
```
cd code
npm run dev
```

# Backend

## Docker
Start up all the docker containers.
```
sudo docker-compose up # wordt uitgezet op Linux, maar moet blijven runnen; is de database
```

Show running docker containers.
```
sudo docker ps
```
This should list the containers named "post_gres" and "pgAdmin".


## Dotnet
Install the migration tool.
```
cd Server.Api
dotnet tool install --global dotnet-ef
```

Delete the migrations folder, clear and refill the database.
```
cd code
npm run migrations
```

Optionally, follow the instructions at [Microsoft's help page](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio#ssl-linux) to enforce https on your browser.

Run the backend.
```
cd code
npm run server
```

# Frontend

# React native
Run the frontend.
```
cd code
npm run client
```

## PgAdmin
Once docker is running, you can access pgAdmin on [localhost:8080](http://localhost:8080). On the website, login with the following credentials.
```
Email = admin@stunet.be
password = admin
```

### Connect to database in pgAdmin
```
host = postgres
port = 5433
username = postgres
password = postgres
```

## Postgres database
Via the terminal (`dotnet`) or via pgAdmin, you can access [localhost:5433](http://localhost:5433).
```
port = 5433
username = postgres
password = postgres
db_name = stunet
```
