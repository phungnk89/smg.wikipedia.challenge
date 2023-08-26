This repository contains an example automation test project that utilizes C#, Selenium, and SpecFlow to perform automated testing of a web application.

## Table of Contents

- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Folder Structure](#folder-structure)
- [Writing Tests](#writing-tests)
- [Executing Tests](#executing-tests)
- [Reporting](#reporting)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project demonstrates how to set up and write automated tests for a web application using C#, Selenium, and SpecFlow. The combination of these technologies allows for writing tests in a behavior-driven development (BDD) style, making it easier to collaborate with non-technical stakeholders.

## Prerequisites

Before you begin, ensure you have the following installed:

- Visual Studio (or another C# IDE of your choice)
- .NET Framework
- SpecFlow extension for Visual Studio
- Selenium WebDriver

## Installation

1. Clone this repository to your local machine:
```
https://github.com/phungnk89/smg.wikipedia.challenge.git
```
2. Open the project in your preferred IDE (e.g., Visual Studio).

3. Restore the NuGet packages to install the necessary dependencies.

## Usage

1. Navigate to the `Features` folder in the project. Here, you'll find `.feature` files that define the behavior-driven scenarios.

2. Write your feature files using Gherkin syntax, describing the behavior you want to test.

3. Implement the step definitions for the feature files. These are located in the `StepDefinitions` folder.

4. Create page object models (POMs) in the `Elements` folder to interact with different parts of the web application.

5. Organize your test logic and assertions in the `Steps` folder within the `StepDefinitions` folder.

## Folder Structure

- `Features`: Contains the `.feature` files written in Gherkin syntax.
- `StepDefinitions`: Contains the implementation of step definitions corresponding to feature files.
- `Elements`: Contains page object models (POMs) for interacting with web elements.
- `Hooks`: Contains SpecFlow hooks for setting up and tearing down test scenarios.
- `Supports`: Contains built-in methods for handling complex steps.
- `Models`: Contains models for complex objects.
- `Assets`: Contains necessary materials or test data for the tests.

## Writing Tests

1. Create a new `.feature` file in the `Features` folder. Define scenarios using Gherkin syntax.

2. Implement step definitions for the scenarios in the respective feature's step definition file.

3. Use the page object models (POMs) from the `Elements` folder to interact with web elements.

4. Write your test logic using Selenium WebDriver methods and assertions.

## Executing Tests

1. Build the solution.

2. Open the Test Explorer in Visual Studio.

3. Discover and run the SpecFlow tests.

## Reporting

SpecFlow+LivingDoc: After the test execution, run the command from SpecFlow LivingDoc CLI to generate HTML test reporting.
```
livingdoc test-assembly SMG.Wikipedia.Challenge.dll -t TestExecution.json --output TestReport.html
```

## Contributing

Contributions are welcome! If you find any issues or want to improve the project, feel free to create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).