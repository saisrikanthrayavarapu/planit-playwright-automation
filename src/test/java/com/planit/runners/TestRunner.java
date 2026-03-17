package com.planit.runners;

import io.cucumber.testng.AbstractTestNGCucumberTests;
import io.cucumber.testng.CucumberOptions;
import org.testng.annotations.DataProvider;

@CucumberOptions(
        features  = "classpath:features",
        glue      = {"com.planit.stepdefs"},
        plugin    = {
                "pretty",
                "html:build/reports/cucumber/cucumber-report.html",
                "json:build/reports/cucumber/cucumber-report.json",
                "junit:build/reports/cucumber/cucumber-report.xml",
                "io.qameta.allure.cucumber7jvm.AllureCucumber7Jvm"
        },
        monochrome = true
)
public class TestRunner extends AbstractTestNGCucumberTests {

    /**
     * Override the data provider to allow parallel execution in the future.
     * Currently sequential (parallel = false).
     */
    @Override
    @DataProvider(parallel = false)
    public Object[][] scenarios() {
        return super.scenarios();
    }
}
