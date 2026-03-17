pipeline {
    agent any

    tools {
        jdk 'JDK17'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build and Test') {
            steps {
                sh './gradlew clean test build'
            }
        }
    }

    post {
        always {
            junit 'build/reports/cucumber/cucumber-report.xml'

            publishHTML(target: [
                allowMissing         : false,
                alwaysLinkToLastBuild: true,
                keepAll              : true,
                reportDir            : 'build/reports/cucumber',
                reportFiles          : 'cucumber-report.html',
                reportName           : 'Cucumber HTML Report'
            ])

            archiveArtifacts artifacts: 'build/reports/**/*', fingerprint: true
        }
    }
}
