Make migration
```dotnet ef migrations add RefreshTokens -p Infrastructure -s API -o Data/Migrations```

Pull graphql schema
```dotnet run -- schema export --output schema.graphql```