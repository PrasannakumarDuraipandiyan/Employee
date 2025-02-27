var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Department> departments =
[
    new (101, "HR"),
    new (102, "IT"),
    new (103, "Finance"),
    new (104, "Marketing"),
    new (105, "Sales")
];

app.MapGet("/api/departments", async () =>
{
    await Task.Delay(2000);

    return Results.Ok(departments);
});

app.Run();

record Department(int Id, string Name);
