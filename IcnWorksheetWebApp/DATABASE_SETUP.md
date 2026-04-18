# EF Core Database & Patient Repository Setup

## Overview
This document describes the Entity Framework Core database configuration and the Patient repository implementation for add/update/delete operations.

## Database Configuration

### ApplicationDbContext Updates
The `ApplicationDbContext` has been configured with:

1. **DbSets**
   - `Patients` - Patient records
   - `WardInfectionSurveillance` - Ward infection records

2. **Patient Entity Configuration**
   - Primary Key: `Id`
   - First Name: Required, Max 100 chars
   - Last Name: Required, Max 100 chars
   - Hospital Number (HN): Required, Max 50 chars, **Unique Index**
   - CreatedAt: Auto-set to current timestamp

3. **WardInfectionSurveillance Entity Configuration**
   - Primary Key: `Id`
   - Ward Name: Required, Max 100 chars
   - Hospital Number: Max 50 chars
   - Patient Name: Max 200 chars
   - CreatedAt: Auto-set to current timestamp

## Database Provider
- **Provider**: SQLite
- **Connection String**: `Data Source=app.db`
- **Location**: Root directory (`app.db` file)

## Repository Pattern

### IPatientRepository Interface
Extends `IRepository<Patient>` with patient-specific operations:

```csharp
public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByHospitalNumberAsync(string hospitalNumber);
    Task<bool> HospitalNumberExistsAsync(string hospitalNumber);
    Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm);
}
```

**Methods:**
- `GetByHospitalNumberAsync()` - Retrieve patient by HN
- `HospitalNumberExistsAsync()` - Check for duplicate HN
- `SearchByNameAsync()` - Search by first/last name

### PatientRepository Implementation
Inherits from `Repository<Patient>` and implements `IPatientRepository`:

**Features:**
- Queries directly against the database context
- Async operations for database calls
- LINQ-based searching and filtering
- Case-insensitive search functionality

## Dependency Injection Setup

### Program.cs Configuration
```csharp
// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Generic repository for all entities
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Specific repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
```

## Page Model Integration

### Add Patient (Pages/Patient/Add.cshtml.cs)
- Validates required fields
- Checks for duplicate Hospital Numbers
- Creates new Patient entity
- Saves to database via repository
- Returns success message

### Index Patient (Pages/Patient/Index.cshtml.cs)
- Fetches all patients from database
- Maps entities to DTOs
- Returns sorted list

### View Patient (Pages/Patient/View.cshtml.cs)
- Retrieves patient by ID
- Maps entity to DTO
- Returns 404 if not found

### Edit Patient (Pages/Patient/Edit.cshtml.cs)
- Loads patient by ID
- Validates changes
- Checks for duplicate HN if changed
- Updates entity
- Saves to database

### Delete Patient (Pages/Patient/Delete.cshtml.cs)
- Retrieves patient for verification
- Deletes from database
- Returns confirmation message

## Database Operations

### CRUD Operations

#### Create
```csharp
var patient = new Patient { 
    FirstName = "John", 
    LastName = "Doe", 
    HospitalNumber = "HN123" 
};
await _patientRepository.AddAsync(patient);
await _patientRepository.SaveChangesAsync();
```

#### Read
```csharp
// Get by ID
var patient = await _patientRepository.GetByIdAsync(1);

// Get by HN
var patient = await _patientRepository.GetByHospitalNumberAsync("HN123");

// Get all
var patients = await _patientRepository.GetAllAsync();

// Search by name
var results = await _patientRepository.SearchByNameAsync("John");
```

#### Update
```csharp
var patient = await _patientRepository.GetByIdAsync(1);
patient.FirstName = "Jane";
patient.UpdatedAt = DateTime.UtcNow;
await _patientRepository.UpdateAsync(patient);
await _patientRepository.SaveChangesAsync();
```

#### Delete
```csharp
await _patientRepository.DeleteAsync(patientId);
await _patientRepository.SaveChangesAsync();
```

## Data Validation

### Server-Side Validation
- FirstName: Required, non-empty
- LastName: Required, non-empty
- HospitalNumber: Required, non-empty, **unique**

### Database Constraints
- Unique index on HospitalNumber
- Required fields enforced at DB level
- Max length constraints enforced

## Migration Notes

### Initial Migration
To create the initial database:

```powershell
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Future Changes
For schema changes:
```powershell
# Add new migration
dotnet ef migrations add DescribeYourChange

# Update database
dotnet ef database update
```

## Build Status
✅ Successfully builds with .NET 10
✅ All page models integrated with repository
✅ Database context configured with entities
✅ Dependency injection set up

## Next Steps

1. **Run Initial Migration**
   - Execute `dotnet ef migrations add InitialCreate`
   - Execute `dotnet ef database update`

2. **Test Patient Management**
   - Navigate to `/Patient/Index`
   - Add, edit, view, and delete patients
   - Verify HN uniqueness constraint

3. **Extend Patient Entity** (if needed)
   - Add DOB (Date of Birth)
   - Add Phone Number
   - Add Email
   - Add Address fields

4. **Create Service Layer** (optional)
   - IPatientService interface
   - PatientService implementation
   - Business logic separation

5. **Add Ward Infection Surveillance Database**
   - Create similar repository pattern
   - Implement CRUD operations
   - Add validation rules
