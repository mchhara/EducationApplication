using EducationAPI;
using EducationAPI.Entities;
using System.Reflection;
using EducationAPI.Services.EducationalSubject;
using EducationAPI.Middleware;
using EducationAPI.Services;
using EducationAPI.Services.UserServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using EducationAPI.Models.User.Validators;
using FluentValidation;
using EducationAPI.Models.User;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EducationDbContext>();
builder.Services.AddScoped<EducationalSubjectSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IEducationalSubjectServices, EducationalSubjectServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();


var app = builder.Build();


// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<EducationalSubjectSeeder>();

seeder.Seed();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
