using EducationAPI;
using EducationAPI.Entities;
using System.Reflection;
using EducationAPI.Services;
using EducationAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EducationDbContext>();
builder.Services.AddScoped<EducationalMaterialSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IEducationalMaterialServices, EducationalMaterialServices>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();


var app = builder.Build();


// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<EducationalMaterialSeeder>();

seeder.Seed();


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
