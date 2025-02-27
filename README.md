# Employee

## Overview
The Employee project is a web application built using Blazor, targeting .NET 9. It provides a comprehensive solution for managing employee data, including features for adding, updating, and viewing employee information. The project leverages modern web technologies and follows best practices to ensure a robust and maintainable codebase.

## Features
- **Employee Management**: Add, update, and view employee details.
- **Validation**: Utilizes FluentValidation for input validation.
- **API Documentation**: Integrated with Swashbuckle for generating OpenAPI documentation.
- **Responsive Design**: Built with Bootstrap and MudBlazor for a responsive and user-friendly interface.

## Installation
To set up the project locally, follow these steps:

1. **Clone the repository**:
    
2. **Build the solution**:
    Open the solution in Visual Studio 2022 and build the project to restore the necessary packages.

3. **Run the API project**:
    - Set the `EmployeeWebAPI` project as the startup project.
    - Run the project. The API will be available at `http://localhost:5001`.

4. **Run the UI project**:
    - Set the `EmployeeUI` project as the startup project.
    - Run the project. The UI will be available at `http://localhost:5000`.

## Usage
Once the application is running, you can access the following features:

- **Employee List**: View a list of all employees.
- **Add Employee**: Add a new employee to the system.
- **Edit Employee**: Update existing employee details.
- **Delete Employee**: Remove an employee from the system.

## API Documentation
The API documentation is available at the following URL:
- **Swagger UI**: `http://localhost:5001/swagger`

## Dependencies
The project relies on the following NuGet packages:

- `Microsoft.AspNetCore.OpenApi` (v9.0.2)
- `FluentValidation.AspNetCore` (v11.3.0)
- `Swashbuckle.AspNetCore` (v6.4.0)
- `coverlet.collector` (v6.0.2)
- `Microsoft.NET.Test.Sdk` (v17.12.0)
- `Moq` (v4.20.72)
- `RichardSzalay.MockHttp` (v7.0.0)
- `Shouldly` (v4.3.0)
- `xunit` (v2.9.2)
- `xunit.runner.visualstudio` (v2.8.2)

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

## Contact
For any questions or support, please contact [prasanna_durai@yahoo.co.in].
