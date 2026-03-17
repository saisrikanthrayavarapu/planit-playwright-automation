# Planit Jupiter Toys — Java Selenium Cucumber TestNG Automation

End-to-end test automation suite for the [Jupiter Toys](http://jupiter.cloud.planittesting.com) web application, built with:

- **Java 17**
- **Selenium WebDriver 4.21**
- **Cucumber 7.18 (BDD)**
- **TestNG 7.10**
- **WebDriverManager 5.8** (automatic ChromeDriver management)
- **AssertJ 3.25** (fluent assertions)
- **Gradle 8+**

---

## Prerequisites

| Tool | Version |
|------|---------|
| Java JDK | 17+ |
| Gradle | 8+ (or use the Gradle Wrapper `./gradlew`) |
| Google Chrome | Latest stable |
| ChromeDriver | Managed automatically by WebDriverManager |

---

## Project Structure

```
planit-java-automation/
├── src/
│   ├── main/java/com/planit/          # (reserved for production code)
│   └── test/
│       ├── java/com/planit/
│       │   ├── pages/                 # Page Object Model classes
│       │   │   ├── BasePage.java      # Base page with shared Selenium helpers
│       │   │   ├── HomePage.java      # Home page — navigation links
│       │   │   ├── ContactPage.java   # Contact page — form fields & validation
│       │   │   ├── ShopPage.java      # Shop page — add products to cart
│       │   │   └── CartPage.java      # Cart page — verify prices & totals
│       │   ├── stepdefs/              # Cucumber step definitions
│       │   │   ├── Hooks.java         # @Before / @After (driver init & quit)
│       │   │   ├── ContactSteps.java  # Steps for TC1 & TC2
│       │   │   └── CartSteps.java     # Steps for TC3
│       │   ├── runners/
│       │   │   └── TestRunner.java    # TestNG + Cucumber runner
│       │   └── utils/
│       │       ├── DriverManager.java # ThreadLocal WebDriver management
│       │       └── PriceUtils.java    # Price rounding utility
│       └── resources/
│           └── features/
│               └── jupiter_toys.feature  # All BDD scenarios
├── build.gradle                       # Gradle build configuration
├── settings.gradle
├── testng.xml                         # TestNG suite definition
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

### Run all tests

```bash
./gradlew clean test build
```

### Run a specific tag

```bash
# Run TC1 only
./gradlew clean test build -Dcucumber.filter.tags="@TC1"

# Run TC2 only
./gradlew clean test build -Dcucumber.filter.tags="@TC2"

# Run TC3 only
./gradlew clean test build -Dcucumber.filter.tags="@TC3"

# Run TC1 and TC2
./gradlew clean test build -Dcucumber.filter.tags="@TC1 or @TC2"
```

### On Windows (cmd/PowerShell)

```cmd
gradlew.bat clean test build
gradlew.bat clean test build -Dcucumber.filter.tags="@TC1"
```

---

## Reports

After the build, reports are generated at:

| Report | Location |
|--------|----------|
| Cucumber HTML | `build/reports/cucumber/cucumber-report.html` |
| Cucumber JSON | `build/reports/cucumber/cucumber-report.json` |
| JUnit XML | `build/reports/cucumber/cucumber-report.xml` |
| TestNG HTML | `build/reports/tests/test/index.html` |
| Allure Results (raw) | `build/allure-results/` |
| Allure HTML Report | `build/reports/allure-report/index.html` |

Open the HTML report in a browser to see detailed test results with step-by-step output.

### Allure Reports

Allure provides enhanced test reporting with detailed analytics and visual test execution history.

**Generate Allure report:**

```bash
./gradlew allureReport
```

This will generate the HTML report at `build/reports/allure-report/index.html`. Open it in a browser to view:
- Test execution timeline
- Test results with detailed logs
- Screenshots and attachments
- Test history and trends
- Severity classifications

**On Windows:**
```cmd
gradlew.bat allureReport
```

---

## CI/CD — Jenkins

The `Jenkinsfile` defines a declarative pipeline:

1. **Checkout** — checks out the source code from SCM
2. **Build and Test** — runs `./gradlew clean test build`
3. **Post-build (always)**:
   - Publishes JUnit XML results
   - Publishes Cucumber HTML report (requires [HTML Publisher Plugin](https://plugins.jenkins.io/htmlpublisher/))
   - Archives all artifacts under `build/reports/`

### Jenkins Setup

1. Create a new Pipeline job in Jenkins
2. Point it to your SCM repository
3. Ensure the `JDK17` tool is configured in **Manage Jenkins → Tools**
4. Install the **HTML Publisher Plugin** for the HTML report step

---

## Configuration

### Implicit Wait
5 seconds (configured in `DriverManager.initDriver()`)

### Explicit Wait
- Visibility: 15 seconds
- Invisibility: 10 seconds

Both are defined in `BasePage`.

### ChromeDriver Options
- `--no-sandbox` — required in Docker/Jenkins environments
- `--disable-dev-shm-usage` — prevents memory issues in CI
- `--start-maximized` — maximises browser window

---

## Troubleshooting

**ChromeDriver version mismatch**  
WebDriverManager automatically downloads the matching ChromeDriver. If it fails, try:
```java
WebDriverManager.chromedriver().browserVersion("stable").setup();
```

**Tests timing out**  
Increase explicit waits in `BasePage.waitForVisible()` / `BasePage.waitForHidden()`.

**Gradle not found**  
Use the Gradle Wrapper script included in the project:
- macOS/Linux: `./gradlew`
- Windows: `gradlew.bat`

---

## Contact

| Detail | Information |
|--------|-------------|
| Email | saisrikanth.rayavarapu@gmail.com |
| Mobile | +64 224769889 |
