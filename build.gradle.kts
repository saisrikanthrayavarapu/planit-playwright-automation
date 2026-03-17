plugins {
    java
    eclipse
    id("io.qameta.allure") version "2.11.2"
}

group = "com.planit"
version = "1.0.0"

java {
    sourceCompatibility = JavaVersion.VERSION_17
    targetCompatibility = JavaVersion.VERSION_17
}

repositories {
    mavenCentral()
}

dependencies {
    testImplementation("io.cucumber:cucumber-java:7.18.0")
    testImplementation("io.cucumber:cucumber-testng:7.18.0")
    testImplementation("io.cucumber:cucumber-picocontainer:7.18.0")
    testImplementation("org.testng:testng:7.10.2")
    testImplementation("org.seleniumhq.selenium:selenium-java:4.21.0")
    testImplementation("io.github.bonigarcia:webdrivermanager:5.8.0")
    testImplementation("org.assertj:assertj-core:3.25.3")
    
    // Allure reporting
    testImplementation("io.qameta.allure:allure-cucumber7-jvm:2.26.0")
    testImplementation("io.qameta.allure:allure-testng:2.26.0")
}

tasks.test {
    useTestNG {
        suites("testng.xml")
        useDefaultListeners(true)
    }

    // Make sure the working directory is the project root so feature paths resolve
    workingDir = project.projectDir

    // setScanForTestClasses(false) — use Java method call to bypass Kotlin DSL visibility issue.
    // This tells Gradle NOT to pre-filter by @Test annotations so TestRunner
    // (which inherits @Test from AbstractTestNGCucumberTests) is included.
    setScanForTestClasses(false)

    // Include ONLY the Cucumber TestNG runner
    include("com/planit/runners/TestRunner.class")

    // Pass through all system properties
    systemProperties(System.getProperties().mapKeys { it.key.toString() })

    System.getProperty("cucumber.filter.tags")?.let {
        systemProperty("cucumber.filter.tags", it)
    }

    System.getProperty("cucumber.options")?.let {
        systemProperty("cucumber.options", it)
    }

    // Allure results directory
    systemProperty("allure.results.directory", "${project.buildDir}/allure-results")

    testLogging {
        events("passed", "skipped", "failed")
        showStandardStreams = true
    }

    mustRunAfter(tasks.clean)
}

// Make build depend on test
tasks.build {
    dependsOn(tasks.test)
}
