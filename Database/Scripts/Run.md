
1. Create migration: (use different MigrationName alway to use)
    dotnet msbuild -target:AddMigration -property:MigrationName=InitialUserMigration

2. Update database:
    dotnet msbuild -target:UpdateDatabase
