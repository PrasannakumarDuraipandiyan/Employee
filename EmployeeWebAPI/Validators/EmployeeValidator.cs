using EmployeeWebAPI.Models;
using EmployeeWebAPIGatewayAPI.Constants;
using FluentValidation;
using System.Text.Json;

namespace EmployeeWebAPI.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EmployeeValidator(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        RuleFor(emp => emp.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(emp => emp.DepartmentId)
            .Must(IsValidDepartmentId).WithMessage("Invalid DepartmentId. It does not exist.");
    }

    private bool IsValidDepartmentId(int departmentId)
    {
        using var departmentClient = _httpClientFactory.CreateClient(HttpClientNames.DepartmentService);

        var departments = departmentClient.GetFromJsonAsync<List<Department>>("").GetAwaiter().GetResult();

        return departments?.Any(d => d.Id == departmentId) ?? false;
    }
}
