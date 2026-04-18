# EF Core Migrations - Quick Start Guide

## Prerequisites
Make sure you have EF Core tools installed:

```powershell
dotnet tool install --global dotnet-ef
```

## Create Initial Database

### Step 1: Create Migration
Navigate to the project root and run:

```powershell
dotnet ef migrations add InitialCreate
```

This creates a migration file in the `Migrations` folder with the database schema.

### Step 2: Update Database
Apply the migration to create the database:

```powershell
dotnet ef database update
```

The `app.db` SQLite database file will be created in the root directory.

## Verify Database Creation

After running the migration, you should see:
- `app.db` file created in `C:\Users\knott\Documents\Projects\icn-worksheet\IcnWorksheetWebApp\`
- Database tables created: `Patients`, `WardInfectionSurveillance`

## Common Commands

### View Migration Status
```powershell
dotnet ef migrations list
```

### Rollback to Previous Migration
```powershell
dotnet ef database update <PreviousMigrationName>
```

### Remove Last Migration (before applying)
```powershell
dotnet ef migrations remove
```

### Drop Database and Recreate
```powershell
dotnet ef database drop
dotnet ef database update
```

### View Generated SQL
```powershell
dotnet ef migrations script
```

## Adding New Migrations

When you modify entities (add properties, change types, etc.):

1. Update the entity class
2. Create migration:
   ```powershell
   dotnet ef migrations add <DescriptionOfChange>
   ```
3. Apply migration:
   ```powershell
   dotnet ef database update
   ```

## Troubleshooting

### Error: "No database provider has been configured"
- Ensure `ApplicationDbContext` is registered in `Program.cs` ✅ (Already done)

### Error: "Unable to create an object of type 'ApplicationDbContext'"
- Make sure DbContext constructor accepts `DbContextOptions<ApplicationDbContext>` ✅ (Already done)

### Error: "The specified module could not be found"
- Install the latest EF Core package:
  ```powershell
  dotnet add package Microsoft.EntityFrameworkCore.Sqlite
  ```

## Database Location
- **File**: `app.db`
- **Path**: Root of IcnWorksheetWebApp directory
- **Type**: SQLite database

## Next: Test Patient Management

Once migrations are complete, test the patient management:

1. Start the application
2. Navigate to `http://localhost:5000/Patient/Add` (or appropriate port)
3. Create a test patient
4. Verify it appears in the list
5. Test edit and delete operations

Good luck! 🚀
