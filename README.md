# Planit Jupiter Toys — C# Playwright Reqnroll NUnit Automation

End-to-end test automation suite for the [Jupiter Toys](http://jupiter.cloud.planittesting.com) web application, built with:

- **C#**
- **.NET 8.0**
- **Playwright** (browser automation)
- **Reqnroll** (BDD framework, formerly SpecFlow)
- **NUnit** (testing framework)
- **FluentAssertions** (readable assertions)

---

## Prerequisites

| Tool | Version |
|------|---------|
| .NET SDK | 8.0+ |
| Google Chrome | Latest stable |
| Microsoft Edge | Latest stable (optional) |

---

## Project Structure

```
planit-C#-automation/
├── PlanitAutomation/
│   ├── PlanitAutomation.csproj        # .NET project file
│   ├── reqnroll.json                  # Reqnroll configuration
│   ├── Features/
│   │   ├── JupiterToys.feature        # BDD feature file
│   │   └── JupiterToys.feature.cs     # Generated step definitions
│   ├── Pages/
│   │   ├── BasePage.cs                # Base page with shared helpers
│   │   ├── HomePage.cs                # Home page — navigation links
│   │   ├── ContactPage.cs             # Contact page — form fields & validation
│   │   ├── ShopPage.cs                # Shop page — add products to cart
│   │   └── CartPage.cs                # Cart page — verify prices & totals
│   ├── StepDefinitions/
│   │   ├── Hooks.cs                   # @Before / @After (driver init & quit)
│   │   ├── ContactSteps.cs            # Steps for TC1 & TC2
│   │   └── CartSteps.cs               # Steps for TC3
│   ├── Utils/
│   │   ├── BrowserManager.cs          # Browser management
│   │   └── PriceUtils.cs              # Price rounding utility
│   └── bin/Debug/net8.0/              # Build output
├── NuGet.Config                       # NuGet package sources
├── planit-C#-automation.sln           # Solution file
├── Jenkinsfile                        # CI/CD pipeline definition
└── README.md
```

---

## Test Scenarios

| Tag  | Scenario |
|------|----------|
| @TC1 | Contact form shows validation errors on empty submit, clears them after filling fields |
| @TC2 | Successful contact form submission — runs 5 times with different data (Scenario Outline) |
| @TC3 | Cart subtotals and grand total are calculated correctly |

---

## How to Run

### Prerequisites Setup

1. Install .NET 8.0 SDK
2. Install Playwright browsers:
   ```bash
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright install
   ```

### Run all tests

```bash
dotnet test
```

### Run a specific tag

```bash
# Run TC1 only
dotnet test --filter "Category=TC1"

# Run TC2 only
dotnet test --filter "Category=TC2"

# Run TC3 only
dotnet test --filter "Category=TC3"
```

### Build the project

```bash
dotnet build
```

---

## Reporting

Test execution generates the following outputs:

- **Test Results**: NUnit TRX files in `TestResults/` directory
- **Screenshots**: On test failure, full-page screenshots are captured and saved to `TestResults/Screenshots/`
- **Logs**: Detailed execution logs are written to console and saved to `TestResults/Logs/` with timestamps

To view test results after running tests:

```bash
# View test results in browser (if using Visual Studio Test Explorer)
# Or check the TRX files in TestResults/
```

---

## CI/CD

The `Jenkinsfile` defines a pipeline that:

1. Checks out the source code
2. Runs `dotnet test` with TRX logging
3. Publishes test results and archives artifacts

---

## GitHub Repository Setup

To upload this project to a new GitHub repository:

1. Create a new repository on GitHub (e.g., `planit-csharp-automation`)
2. Copy the repository URL
3. Run the following commands in your terminal:

```bash
git remote set-url origin <your-new-repo-url>
git push -u origin master
```

This will push the current committed changes to your new GitHub repository.

| Detail | Information |
|--------|-------------|
| Email | saisrikanth.rayavarapu@gmail.com |
| Mobile | +64 224769889 |
