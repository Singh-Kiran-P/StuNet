# **StuNet**

# Frontend
This section needs to be updated; is not necessary.
```
npm run i
npm run dev
```

# Backend

## Docker
To start up all the docker containers, use the following command:
```
sudo docker-compose up # wordt uitgezet op Linux, maar moet blijven runnen; is de database
```

To show running docker containers:
```
sudo docker ps
```
<!-- TODO: use correct names -->
This should list the containers named "post_gres" and "pgAdmin".


## Dotnet
To install the migration tool, run
```
dotnet tool install --global dotnet-ef
```

Run the following commands in the [Server.Api folder](/code/Server.Api). These commands will delete the migrations folder, clear the database and refill the database.
```
dotnet ef database update 0
dotnet ef migrations remove
dotnet ef database drop -f -v
dotnet ef migrations add Initial
dotnet ef database update
```

Optionally, follow the instructions at [Microsoft's help page](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio#ssl-linux) to enforce https on your browser.

Finally, run the backend with the following commands.

```
dotnet build
dotnet run
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