using EmployeeWebAPI.Middleware;
using EmployeeWebAPI.Validators;
using EmployeeWebAPIGatewayAPI.Constants;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(HttpClientNames.EmployeeService, client =>
{
    client.BaseAddress = new Uri("http://localhost:5002/api/employees/");
});

builder.Services.AddHttpClient(HttpClientNames.DepartmentService, client =>
{
    client.BaseAddress = new Uri("http://localhost:5003/api/departments/");
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    // Define the security scheme (API Key)
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Description = "Enter your API Key here",
        Scheme = "ApiKeyScheme"
    });

    // Apply the security requirement globally (default for all requests)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
