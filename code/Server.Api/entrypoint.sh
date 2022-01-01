#!/bin/bash

set -e
run_cmd="dotnet run"

export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update 0; do
>&2 echo " database update"
sleep 1
done

until dotnet ef migrations remove; do
>&2 echo "migrations remove"
sleep 1
done

until dotnet ef database drop -f; do
>&2 echo "database drop"
sleep 1
done

until dotnet ef migrations add Initial ; do
>&2 echo "migrations add Initial"
sleep 1
done

until dotnet ef database update ; do
>&2 echo "database update"
sleep 1
done

>&2 echo "Database is up - executing command"
exec $run_cmd
