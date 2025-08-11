using Microsoft.EntityFrameworkCore;
using FreelancerDirectory.Infrastructure.Persistence;
using FreelancerDirectory.Application.Interfaces;
using FreelancerDirectory.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFreelancerRepository, FreelancerRepository>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Only use SQL Server in non-test environments
if (!builder.Environment.EnvironmentName.Equals("Test", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<FreelancerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS (must be before UseAuthorization)
app.UseCors("AllowUI");

app.UseAuthorization();
app.MapControllers();

app.Run();

namespace FreelancerDirectory.API
{
    public partial class Program { }
}