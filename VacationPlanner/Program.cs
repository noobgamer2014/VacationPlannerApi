using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VacationPlanner.Services;
using VacationPlanner.Models;
using Microsoft.AspNetCore.Identity;
using VacationPlanner.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Token creator
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<TokenService>();
// Register the DbContext with a connection string from your configuration (appsettings.json)
builder.Services.AddDbContext<VacationPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VacationPlannerDatabase")));

// Add the user service for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IVacationService, VacationService>();
// Register other services like mailing service if you have any
//builder.Services.AddScoped<IMailingService, MailingService>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<AuthenticationController>();
builder.Services.AddTransient<VacationController>();

// Add controllers (assuming you're using MVC)
builder.Services.AddControllers();

// Add any other services needed by your application here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
var app = builder.Build();

// Use appropriate middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
// Use static files if serving any (e.g., Angular front-end)
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");
// Use routing
app.UseRouting();

// Use authentication and authorization if needed
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Start the application
app.Run();
