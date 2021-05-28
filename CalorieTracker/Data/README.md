## Create Migration

dotnet ef migrations add v1 -c DbContext -p CalorieTracker -o Data/Migrations
dotnet ef migrations add v2 -c DbContext -p CalorieTracker -o Data/Migrations
dotnet ef migrations add v3 -c DbContext -p CalorieTracker -o Data/Migrations
dotnet ef migrations add v4 -c DbContext -p CalorieTracker -o Data/Migrations

## Update Database
dotnet ef database update -c DbContext -p CalorieTracker

## Remove Migration
dotnet ef migrations remove -c DbContext -p CalorieTracker