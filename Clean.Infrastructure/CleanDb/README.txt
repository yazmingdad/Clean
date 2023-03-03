#Clean Database

# Entity Framework Core
## NOTES: 
1. migrations will use the connection string defined in Clean.Infrastructure/appsettings.json
      as this is the connection string referenced by DesignTimeDbContextFactory
1. Consult Microsoft documentation on Entity Framework Core Code First migrations for more information on migrations.

## To create migrations for the first time:
* Add-Migration -Name "InitialCleanDbCreation" -OutputDir "CleanDb\Migrations" -Context "Clean.Infrastructure.CleanDb.Models.CleanContext" -Project "Clean.Infrastructure"

## To run the migrations:
* Update-Database -Context "Clean.Infrastructure.CleanDb.Models.CleanContext" -Project "Clean.Infrastructure"