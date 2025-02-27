var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<Employee> employees =
[
   new (1, "Prasannakumar", 101),
   new (2, "Duraipandiyan", 102),
   new (3, "John Doe", 103),
   new (4, "Jane Smith", 104),
   new (5, "Michael Johnson", 105),
   new (6, "Emily Davis", 101),
   new (7, "David Wilson", 102),
   new (8, "Sarah Brown", 103),
   new (9, "James Taylor", 104),
   new (10, "Linda Martinez", 105),
   new (11, "Robert Anderson", 101),
   new (12, "Patricia Thomas", 102),
   new (13, "Charles Jackson", 103),
   new (14, "Barbara White", 104),
   new (15, "Christopher Harris", 105),
   new (16, "Jessica Martin", 101),
   new (17, "Daniel Thompson", 102),
   new (18, "Nancy Garcia", 103),
   new (19, "Matthew Martinez", 104),
   new (20, "Karen Robinson", 105)
];

app.MapGet("/api/employees", async () =>
{
    await Task.Delay(1000);

    return Results.Ok(employees);
});

app.MapPost("/api/employees", (Employee emp) =>
{

    var newEmp = emp with { Id = employees.Max(e => e.Id) + 1 };

    employees.Add(newEmp);

    return Results.Ok(newEmp);
});

app.MapPut("/api/employees/{id:int}", (int id, Employee emp) =>
{
    var index = employees.FindIndex(e => e.Id == id);

    if (index == -1) return Results.NotFound();

    employees[index] = emp;

    return Results.Ok(emp);
});

app.MapDelete("/api/employees/{id:int}", (int id) =>
{
    employees.RemoveAll(e => e.Id == id);

    return Results.Ok();
});

app.Run();

record Employee(int Id, string Name, int DepartmentId);
