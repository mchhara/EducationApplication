using EducationAPI;
using EducationAPI.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EducationDbContext>();
builder.Services.AddScoped<EducationalMaterialSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();


// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<EducationalMaterialSeeder>();

seeder.Seed();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
