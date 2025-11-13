using Api.Data;
using Api.Mapping;
using Api.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework with InMemory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CrmApiDb"));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(ContactProfile));

// Configure FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CRM API",
        Version = "v1",
        Description = "API for managing contacts"
    });
    
    // Exclude error endpoint from Swagger
    c.SwaggerGeneratorOptions.IgnoreObsoleteActions = true;
    
    // Configure schema generation for DateTimeOffset
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

// Configure JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}

// Global exception handling
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDataService.SeedData(context);
}

app.Run();
