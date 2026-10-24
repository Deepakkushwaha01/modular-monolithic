
1. Create migration: (use different MigrationName alway to use)
    dotnet msbuild -target:AddMigration -property:MigrationName=InitialUserMigration

2. Update database:
    dotnet msbuild -target:UpdateDatabase



Load Seed Data with Migration 
dotnet build Core/App.Core.Persistence/App.Core.Persistence.csproj \
  -t:AddMigration \
  -p:MigrationName=AddIdentitySeedData \
  -p:LoadSeedData=true


Load Seed Data with Update 
dotnet build Core/App.Core.Persistence/App.Core.Persistence.csproj \
  -t:UpdateDatabase \
  -p:LoadSeedData=true