pipeline {
    agent any

    tools {
        dotnet 'dotnet-8.0'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build and Test') {
            steps {
                sh 'dotnet test --logger "trx;LogFileName=test-results.trx"'
            }
        }
    }

    post {
        always {
            publishTestResults([
                testResultsFilePattern: '**/test-results.trx',
                testRunTitle: 'Test Results'
            ])

            archiveArtifacts artifacts: '**/TestResults/**/*', fingerprint: true
        }
    }
}
