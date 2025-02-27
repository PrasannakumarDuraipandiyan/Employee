using EmployeeWebAPI.Models;
using EmployeeWebAPIGatewayAPI.Constants;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IValidator<Employee> _validator;

    public EmployeeController(IHttpClientFactory httpClientFactory, IValidator<Employee> validator)
    {
        _httpClientFactory = httpClientFactory;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        using var employeeClient = _httpClientFactory.CreateClient(HttpClientNames.EmployeeService);
        using var departmentClient = _httpClientFactory.CreateClient(HttpClientNames.DepartmentService);

        var employeeTask = employeeClient.GetFromJsonAsync<List<Employee>>("");
        var departmentTask = departmentClient.GetFromJsonAsync<List<Department>>("");

        await Task.WhenAll(employeeTask, departmentTask);

        var employees = await employeeTask;
        var departments = await departmentTask;

        var result = employees?.Select(emp => new EmployeeDetailsDto
        (
            Id: emp.Id,
            Name: emp.Name,
            DepartmentId: emp.DepartmentId,
            DepartmentName: departments?.FirstOrDefault(d => d.Id == emp.DepartmentId)?.Name ?? "Unknown"
        )).ToList();

        return Ok(result);
    }

    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartments()
    {
        using var departmentClient = _httpClientFactory.CreateClient(HttpClientNames.DepartmentService);
        var departments = await departmentClient.GetFromJsonAsync<List<Department>>("");

        if (departments == null)
        {
            return NotFound("Departments not found.");
        }

        return Ok(departments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        using var employeeClient = _httpClientFactory.CreateClient(HttpClientNames.EmployeeService);
        var response = await employeeClient.PostAsJsonAsync("", employee);

        return response.IsSuccessStatusCode ? CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee) : BadRequest("Failed to create employee.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
    {
        if (id != employee.Id) return BadRequest("ID mismatch");

        ValidationResult validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        using var employeeClient = _httpClientFactory.CreateClient(HttpClientNames.EmployeeService);
        var response = await employeeClient.PutAsJsonAsync($"{id}", employee);

        return response.IsSuccessStatusCode ? NoContent() : BadRequest("Failed to update employee.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        using var employeeClient = _httpClientFactory.CreateClient(HttpClientNames.EmployeeService);
        var response = await employeeClient.DeleteAsync($"{id}");

        return response.IsSuccessStatusCode ? NoContent() : NotFound("Employee not found.");
    }
}
