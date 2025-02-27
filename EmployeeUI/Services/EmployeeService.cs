using EmployeeUI.Model;

namespace EmployeeUI.Services;

public class EmployeeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EmployeeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<EmployeeModel>> GetEmployeesAsync()
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        return await httpClient.GetFromJsonAsync<List<EmployeeModel>>("");
    }

    public async Task<EmployeeModel> GetEmployeeAsync(int id)
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        return await httpClient.GetFromJsonAsync<EmployeeModel>($"{id}");
    }

    public async Task<List<DepartmentModel>> GetDepartmentsAsync()
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        return await httpClient.GetFromJsonAsync<List<DepartmentModel>>("departments");
    }

    public async Task CreateEmployeeAsync(EmployeeModel employee)
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        var response = await httpClient.PostAsJsonAsync("", employee);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateEmployeeAsync(int id, EmployeeModel employee)
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        var response = await httpClient.PutAsJsonAsync($"{id}", employee);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        using var httpClient = _httpClientFactory.CreateClient("EmployeeAPI");
        var response = await httpClient.DeleteAsync($"{id}");
        response.EnsureSuccessStatusCode();
    }
}

