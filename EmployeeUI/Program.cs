using EmployeeUI.Components;
using EmployeeUI.Handler;
using EmployeeUI.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add HttpClient for API calls
builder.Services.AddHttpClient<EmployeeService>("EmployeeAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/api/employee/");
})
.AddHttpMessageHandler(() =>
{
    return new ApiKeyHandler(builder.Configuration.GetValue<string>("ApiSettings:ApiKey"));
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddTransient<EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
