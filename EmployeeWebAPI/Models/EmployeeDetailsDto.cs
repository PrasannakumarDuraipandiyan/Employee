namespace EmployeeWebAPI.Models;

/// <summary>
/// Represents detailed information about an employee, including their department details.
/// </summary>
/// <param name="Id">The unique identifier of the employee.</param>
/// <param name="Name">The name of the employee.</param>
/// <param name="DepartmentId">The unique identifier of the department the employee belongs to.</param>
/// <param name="DepartmentName">The name of the department the employee belongs to.</param>
public record EmployeeDetailsDto(
    int Id,
    string Name,
    int DepartmentId,
    string DepartmentName);
