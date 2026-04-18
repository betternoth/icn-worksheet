# EF Core Database & Patient Repository - Complete Implementation

## Summary

You now have a complete **Entity Framework Core database setup** with a **Patient repository** for managing add/update/delete operations. The implementation follows the Clean Architecture pattern and is fully integrated with your Razor Pages application.

---

## What Was Created

### 1. **Database Context (ApplicationDbContext.cs)**
Updated to include:
- ✅ `DbSet<Patient>` for patient records
- ✅ `DbSet<WardInfectionSurveillance>` for ward infection records
- ✅ Entity configuration with constraints and indexes
- ✅ Unique constraint on Hospital Number (HN)
- ✅ Required field validation at DB level
- ✅ Automatic timestamp tracking

### 2. **Repository Interface (IPatientRepository.cs)**
Extends `IRepository<T>` with patient-specific methods:
- ✅ `GetByHospitalNumberAsync()` - Retrieve by HN
- ✅ `HospitalNumberExistsAsync()` - Check uniqueness
- ✅ `SearchByNameAsync()` - Full-text name search

### 3. **Repository Implementation (PatientRepository.cs)**
- ✅ Inherits from generic `Repository<Patient>`
- ✅ Implements all `IPatientRepository` methods
- ✅ Direct database context access for complex queries
- ✅ Async/await pattern throughout

### 4. **Dependency Injection (Program.cs)**
- ✅ Added `IPatientRepository, PatientRepository` registration
- ✅ Generic repository already configured
- ✅ DbContext registered with SQLite provider

### 5. **Updated Page Models**
All patient page models now integrated with repository:

| Page | Changes |
|------|---------|
| **Add.cshtml.cs** | Uses repository to add patient, validates duplicate HN |
| **Index.cshtml.cs** | Fetches all patients from database |
| **View.cshtml.cs** | Retrieves patient by ID from database |
| **Edit.cshtml.cs** | Loads, validates, and updates patient |
| **Delete.cshtml.cs** | Deletes patient and logs operation |

---

## Architecture

```
Presentation Layer (Pages)
    ↓
Page Models (Add/Index/View/Edit/Delete)
    ↓
IPatientRepository (Interface)
    ↓
PatientRepository (Implementation)
    ↓
IRepository<Patient> (Generic)
    ↓
ApplicationDbContext (EF Core)
    ↓
SQLite Database (app.db)
```

---

## Database Schema

### Patients Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Auto-increment |
| FirstName | string(100) | Required |
| LastName | string(100) | Required |
| HospitalNumber | string(50) | Required, **Unique Index** |
| CreatedAt | DateTime | Default: Current timestamp |
| UpdatedAt | DateTime | Nullable |

### Indexes
- Primary Key: `Id`
- Unique: `HospitalNumber`

---

## Key Features

✅ **Unique Hospital Numbers** - Database constraint prevents duplicates
✅ **Async Operations** - All DB operations are async/await
✅ **Clean Architecture** - Separation of concerns maintained
✅ **Type Safety** - Entity alias prevents namespace conflicts
✅ **Error Handling** - Try-catch blocks with logging
✅ **Validation** - Server-side + database constraints
✅ **Logging** - All operations logged for debugging

---

## Getting Started

### 1. Create Initial Database
```powershell
cd C:\Users\knott\Documents\Projects\icn-worksheet\IcnWorksheetWebApp
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 2. Verify Database
- Check for `app.db` file in project root
- Should contain `Patients` and `WardInfectionSurveillance` tables

### 3. Test Patient Management
1. Start the application
2. Navigate to `/Patient/Index`
3. Click "Add New Patient"
4. Fill in: First Name, Last Name, Hospital Number
5. Verify patient is saved in database

### 4. Test Other Operations
- **Edit**: Click edit button, modify fields, save
- **View**: Click view button, see patient details
- **Delete**: Click delete, confirm, patient is removed

---

## Code Examples

### Add Patient
```csharp
// In Add.cshtml.cs
var patient = new PatientEntity
{
    FirstName = Input.FirstName.Trim(),
    LastName = Input.LastName.Trim(),
    HospitalNumber = Input.HospitalNumber.Trim()
};

await _patientRepository.AddAsync(patient);
await _patientRepository.SaveChangesAsync();
```

### Query Patient
```csharp
// Get by ID
var patient = await _patientRepository.GetByIdAsync(id);

// Get by Hospital Number
var patient = await _patientRepository.GetByHospitalNumberAsync("HN123");

// Search by name
var patients = await _patientRepository.SearchByNameAsync("John");
```

### Update Patient
```csharp
patient.FirstName = "Jane";
patient.UpdatedAt = DateTime.UtcNow;
await _patientRepository.UpdateAsync(patient);
await _patientRepository.SaveChangesAsync();
```

### Delete Patient
```csharp
await _patientRepository.DeleteAsync(patientId);
await _patientRepository.SaveChangesAsync();
```

---

## Database Provider

- **Type**: SQLite (lightweight, file-based)
- **Location**: `app.db` in project root
- **Connection String**: `Data Source=app.db`
- **Advantages**: 
  - No external database server needed
  - Great for development
  - Easy to backup (copy file)
  - Can be deployed as single file

---

## Next Steps

### 1. Run Migrations ⭐ (Required)
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 2. Extended Patient Data (Optional)
Add more fields to `Patient.cs`:
```csharp
public DateTime? DateOfBirth { get; set; }
public string? PhoneNumber { get; set; }
public string? Email { get; set; }
public string? Address { get; set; }
```

Then create migration:
```powershell
dotnet ef migrations add AddPatientExtendedData
dotnet ef database update
```

### 3. Ward Infection Database (Optional)
Create similar repository for `WardInfectionSurveillance`:
- `IWardInfectionSurveillanceRepository`
- `WardInfectionSurveillanceRepository`
- Update page models to use repository

### 4. Service Layer (Optional)
Create business logic layer:
- `IPatientService.cs`
- `PatientService.cs`
- Encapsulate business rules

### 5. Add Search/Filter to Index (Optional)
```csharp
public string? SearchTerm { get; set; }

public async Task OnGetAsync()
{
    if (!string.IsNullOrWhiteSpace(SearchTerm))
    {
        Patients = (await _patientRepository.SearchByNameAsync(SearchTerm))
            .Select(MapToDto)
            .ToList();
    }
    else
    {
        Patients = (await _patientRepository.GetAllAsync())
            .Select(MapToDto)
            .ToList();
    }
}
```

---

## Troubleshooting

### Issue: Build fails with namespace conflicts
**Solution**: Using entity alias `PatientEntity = IcnWorksheet.Domain.Patient` ✅ (Already implemented)

### Issue: Migrations not found
**Solution**: 
```powershell
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Issue: Database locked error
**Solution**: Ensure no other application has the `app.db` file open

### Issue: "No database provider has been configured"
**Solution**: Verify `Program.cs` has DbContext configuration ✅ (Already done)

---

## Build Status

✅ **Project builds successfully**
✅ **All dependencies resolved**
✅ **Namespace conflicts resolved**
✅ **Ready for migrations**

---

## File Structure
```
IcnWorksheetWebApp/
├── Data/
│   ├── ApplicationDbContext.cs (Updated)
│   ├── IRepository.cs (Existing)
│   ├── Repository.cs (Existing)
│   ├── IPatientRepository.cs (New)
│   └── PatientRepository.cs (New)
├── Domain/
│   ├── Entity.cs (Existing)
│   └── Patient.cs (Existing)
├── Models/
│   └── PatientDto.cs (Existing)
├── Pages/Patient/
│   ├── Add.cshtml.cs (Updated)
│   ├── Index.cshtml.cs (Updated)
│   ├── View.cshtml.cs (Updated)
│   ├── Edit.cshtml.cs (Updated)
│   └── Delete.cshtml.cs (Updated)
├── Program.cs (Updated)
└── app.db (Will be created after migration)
```

---

## Success Checklist

- [x] ApplicationDbContext configured with DbSets
- [x] Patient entity with constraints
- [x] Unique index on HospitalNumber
- [x] IPatientRepository interface created
- [x] PatientRepository implementation created
- [x] Dependency injection registered
- [x] All page models integrated
- [x] Project builds successfully
- [ ] Run migrations (You need to do this)
- [ ] Test patient management (After migrations)

---

**You're all set! 🎉 Next step: Run the migrations to create the database.**

For detailed migration instructions, see `MIGRATIONS_GUIDE.md`
