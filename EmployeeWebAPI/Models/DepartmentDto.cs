namespace EmployeeWebAPI.Models;

/// <summary>
/// Represents a department with basic information.
/// </summary>
/// <param name="Id">The unique identifier of the department.</param>
/// <param name="Name">The name of the department.</param>
public record Department(
    int Id,
    string Name);
