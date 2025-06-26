# SimpleTestFramework

This is a skeleton for API + Playwright-based UI test automation using .NET 8 and NUnit.

## Structure

- `ApiClients/`: API helper classes
- `UiPages/`: Page Object Model classes
- `ApiTests/`: REST API tests
- `UiTests/`: Playwright UI tests
- `TestBase/`: Base setup classes for UI/API tests
- `Models/`: Request/response DTOs

## Usage

1. Restore dependencies:
   ```
   dotnet restore
   ```

2. Run tests:
   ```
   dotnet test
   ```

Use the agent prompt provided to generate tests.