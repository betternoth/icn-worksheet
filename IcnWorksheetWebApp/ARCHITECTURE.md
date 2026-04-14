# Clean Architecture Structure

This project follows a **Lightweight Clean Architecture** pattern designed for scalability without over-engineering.

## 📁 Folder Structure

```
IcnWorksheet/
├── Pages/                  # Razor Pages (Presentation Layer)
│   ├── Index.cshtml
│   ├── Privacy.cshtml
│   └── ...
│
├── Services/              # Application Layer (Business Logic)
│   ├── IApplicationService.cs
│   └── [Your Use Cases]
│
├── Data/                  # Infrastructure Layer (Data Access)
│   ├── ApplicationDbContext.cs
│   ├── IRepository.cs
│   └── Repository.cs
│
├── Domain/                # Domain Layer (Entities & Rules)
│   ├── Entity.cs
│   └── [Your Entities]
│
├── Models/                # DTOs & ViewModels
│   ├── BaseDto.cs
│   └── ApiResponse.cs
│
└── Program.cs             # Dependency Injection Configuration
```

## 🔹 Layer Responsibilities

### 1. **Pages/** - Presentation Layer
- Razor Pages (`.cshtml` + `.cshtml.cs`)
- Handles HTTP requests/responses
- Input validation
- Calls Application Layer services

### 2. **Services/** - Application Layer
- Business logic & use cases
- DTOs and service interfaces
- Orchestrates Domain and Infrastructure layers
- **Should NOT directly reference Pages or Database**

### 3. **Data/** - Infrastructure Layer
- Entity Framework Core DbContext
- Repository pattern for data access
- Database queries and persistence
- **Implementation detail: can be swapped out**

### 4. **Domain/** - Domain Layer
- Core entities (inherit from `Entity`)
- Value objects
- Business rules & validations
- **Framework-agnostic**

### 5. **Models/** - Data Transfer Objects
- `BaseDto` - Base class for all DTOs
- `ApiResponse<T>` - Standardized response format
- ViewModels for Razor Pages

## 🔧 Dependency Injection (Program.cs)

All services are wired up in `Program.cs`:

```csharp
// Database
builder.Services.AddDbContext<ApplicationDbContext>();

// Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Application Services (add as needed)
builder.Services.AddScoped<IYourService, YourService>();
```

## 📋 Quick Start: Adding a Feature

### 1. Create Domain Entity
```csharp
// Domain/User.cs
public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 2. Create DTO
```csharp
// Models/UserDto.cs
public class UserDto : BaseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 3. Create Application Service
```csharp
// Services/UserService.cs
public interface IUserService : IApplicationService
{
    Task<UserDto?> GetUserAsync(int id);
    Task CreateUserAsync(UserDto dto);
}

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto?> GetUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
    }
    
    public async Task CreateUserAsync(UserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }
}
```

### 4. Register Service (Program.cs)
```csharp
builder.Services.AddScoped<IUserService, UserService>();
```

### 5. Use in Razor Page
```csharp
// Pages/Users/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    
    public IndexModel(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task OnGetAsync()
    {
        // Use service
    }
}
```

## ✅ Best Practices

- **✅ DO**: Keep business logic in Services
- **✅ DO**: Use DTOs to expose only what's needed
- **✅ DO**: Inject dependencies via constructor
- **❌ DON'T**: Call database directly from Pages
- **❌ DON'T**: Expose Domain entities outside Application layer
- **❌ DON'T**: Make Services depend on Presentation layer

## 📚 Resources

- [Microsoft: Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
