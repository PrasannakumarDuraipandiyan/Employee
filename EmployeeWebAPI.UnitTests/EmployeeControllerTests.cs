using EmployeeWebAPI.Controllers;
using EmployeeWebAPI.Models;
using EmployeeWebAPIGatewayAPI.Constants;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RichardSzalay.MockHttp;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace EmployeeWebAPI.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly MockHttpMessageHandler _mockEmployeeHandler;
        private readonly MockHttpMessageHandler _mockDepartmentHandler;
        private readonly Mock<IValidator<Employee>> _mockValidator;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeHandler = new MockHttpMessageHandler();
            _mockDepartmentHandler = new MockHttpMessageHandler();
            _mockValidator = new Mock<IValidator<Employee>>();

            var employeeClient = _mockEmployeeHandler.ToHttpClient();
            employeeClient.BaseAddress = new Uri("http://localhost/api/employees/");

            var departmentClient = _mockDepartmentHandler.ToHttpClient();
            departmentClient.BaseAddress = new Uri("http://localhost/api/departments/");

            var httpClientFactory = new MockHttpClientFactory(employeeClient, departmentClient);
            _controller = new EmployeeController(httpClientFactory, _mockValidator.Object);
        }

        [Fact]
        public async Task GetEmployees_ReturnsOkResult_WithEmployeeDetails()
        {
            // Arrange
            List<Employee> employees = new()
            {
                new Employee ( Id: 1, Name: "John Doe", DepartmentId: 1 )
            };
            List<Department> departments = new()
            {
                new Department ( Id: 1, Name: "HR" )
            };

            _mockEmployeeHandler.When("http://localhost/api/employees/")
                .Respond(HttpStatusCode.OK, JsonContent.Create(employees));

            _mockDepartmentHandler.When("http://localhost/api/departments/")
                .Respond(HttpStatusCode.OK, JsonContent.Create(departments));

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = result.ShouldBeOfType<OkObjectResult>();
            var employeeDetails = okResult.Value.ShouldBeOfType<List<EmployeeDetailsDto>>();
            employeeDetails.Count.ShouldBe(1);
            employeeDetails[0].Name.ShouldBe("John Doe");
            employeeDetails[0].DepartmentName.ShouldBe("HR");
        }

        [Fact]
        public async Task GetDepartments_ReturnsOkResult_WithDepartments()
        {
            // Arrange
            List<Department> departments = new()
            {
                new Department ( Id: 1, Name: "HR" )
            };

            _mockDepartmentHandler.When("http://localhost/api/departments/")
                .Respond(HttpStatusCode.OK, JsonContent.Create(departments));

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var okResult = result.ShouldBeOfType<OkObjectResult>();
            var departmentList = okResult.Value.ShouldBeOfType<List<Department>>();
            departmentList.Count.ShouldBe(1);
            departmentList[0].Name.ShouldBe("HR");
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreatedAtActionResult_WhenEmployeeIsValid()
        {
            // Arrange
            Employee employee = new(1, "John Doe", 1);
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(employee, default))
                .ReturnsAsync(validationResult);

            _mockEmployeeHandler.When("http://localhost/api/employees/")
                .Respond(HttpStatusCode.Created);

            // Act
            var result = await _controller.CreateEmployee(employee);

            // Assert
            var createdResult = result.ShouldBeOfType<CreatedAtActionResult>();
            createdResult.ActionName.ShouldBe(nameof(EmployeeController.GetEmployees));
            createdResult.Value.ShouldBe(employee);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNoContentResult_WhenEmployeeIsValid()
        {
            // Arrange
            Employee employee = new(1, "John Doe", 1);
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(employee, default))
                .ReturnsAsync(validationResult);

            _mockEmployeeHandler.When("http://localhost/api/employees/1")
                .Respond(HttpStatusCode.NoContent);

            // Act
            var result = await _controller.UpdateEmployee(1, employee);

            // Assert
            result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNoContentResult_WhenEmployeeIsDeleted()
        {
            // Arrange
            _mockEmployeeHandler.When("http://localhost/api/employees/1")
                .Respond(HttpStatusCode.NoContent);

            // Act
            var result = await _controller.DeleteEmployee(1);

            // Assert
            result.ShouldBeOfType<NoContentResult>();
        }
    }

    public class MockHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _employeeClient;
        private readonly HttpClient _departmentClient;

        public MockHttpClientFactory(HttpClient employeeClient, HttpClient departmentClient)
        {
            _employeeClient = employeeClient;
            _departmentClient = departmentClient;
        }

        public HttpClient CreateClient(string name)
        {
            return name switch
            {
                HttpClientNames.EmployeeService => _employeeClient,
                HttpClientNames.DepartmentService => _departmentClient,
                _ => throw new ArgumentException("Invalid client name")
            };
        }
    }
}
