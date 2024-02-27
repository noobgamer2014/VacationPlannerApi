using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VacationPlanner.Services;
using VacationPlanner.Models;
using Microsoft.AspNetCore.Identity;

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
// Register the DbContext with a connection string from your configuration (appsettings.json)
builder.Services.AddDbContext<VacationPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VacationPlannerDatabase")));

// Add the user service for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
// Register other services like mailing service if you have any
//builder.Services.AddScoped<IMailingService, MailingService>();


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
