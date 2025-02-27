namespace EmployeeWebAPI.Models;

/// <summary>
/// Represents an employee with basic information.
/// </summary>
/// <param name="Id">The unique identifier of the employee.</param>
/// <param name="Name">The name of the employee.</param>
/// <param name="DepartmentId">The unique identifier of the department the employee belongs to.</param>
public record Employee(
    int Id,
    string Name,
    int DepartmentId);
