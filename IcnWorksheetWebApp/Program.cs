using IcnWorksheet.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Infrastructure Layer: Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=icn;Integrated Security=true;TrustServerCertificate=True;"));

// Infrastructure Layer: Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Infrastructure Layer: Specific Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IWardRepository, WardRepository>();

// Application Layer: Services
// Add your application services here
// builder.Services.AddScoped<IYourService, YourService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
