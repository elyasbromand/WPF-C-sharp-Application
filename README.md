# WPFZooApplication

WPFZooApplication is a Windows Presentation Foundation (WPF) desktop app that uses Entity Framework Core to manage zoos, animals, and their associations.

## Requirements

- .NET 9.0 SDK
- SQL Server instance accessible from the host machine
- Visual Studio 2022/2023 or compatible editor for WPF development

## Setup

1. Clone the repository.
2. Configure the database connection string in an environment variable:
   - PowerShell:

     ```powershell
     $env:CSharpMasterclassConnectionString = "Data Source=SERVER_NAME;Initial Catalog=c-sharp-masterclass;Persist Security Info=True;User ID=USER;Password=PASSWORD;Encrypt=True;Trust Server Certificate=True"
     ```

   - Command Prompt:
     ```cmd
     setx CSharpMasterclassConnectionString "Data Source=SERVER_NAME;Initial Catalog=c-sharp-masterclass;Persist Security Info=True;User ID=USER;Password=PASSWORD;Encrypt=True;Trust Server Certificate=True"
     ```

3. Restore NuGet packages and build the project.

## Run

- Open `WPFZooApplication.sln` in Visual Studio and start the application.
- Or run from the command line in the repository root:

  ```cmd
  dotnet run --project WPFZooApplication\WPFZooApplication.csproj
  ```

## Notes

- The app requires a SQL Server database with the expected schema for `Zoo`, `Animal`, and `ZooAnimal` entities.
- Do not store secrets or connection strings in source code.
- The application reads the database connection string from the `CSharpMasterclassConnectionString` environment variable.
