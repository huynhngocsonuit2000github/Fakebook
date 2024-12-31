<!-- Create Jenkins Agent for tester -->
docker pull jenkins/jenkins:lts
docker run -d --name jenkins -p 8080:8080 -p 50000:50000 jenkins/jenkins:lts


<!-- Create Jenkins Job for tester -->
pipeline {
    agent any

    stages {
        stage('Checkout Code') {
            steps {
                git url: 'https://your-repository-url.git', branch: 'main'
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build Solution') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Run Tests') {
            steps {
                sh 'dotnet test --filter "Category=LoginSuite" --logger:trx'
            }
        }

        stage('Publish Test Results') {
            steps {
                publishHTML([target: [
                    allowMissing: false,
                    alwaysLinkToLastBuild: true,
                    keepAll: true,
                    reportDir: 'TestResults',
                    reportFiles: '*.html',
                    reportName: 'Test Results'
                ]])
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: '**/TestResults/*.trx', allowEmptyArchive: true
        }
        success {
            echo 'Tests passed!'
        }
        failure {
            echo 'Tests failed.'
        }
    }
}


======================================

<!-- Run test -->
dotnet test --filter "DisplayName=LoginTests"


<!-- Create pipeline via Jenkins -->
curl -u username:APITOKEN -X POST "http://jenkins-server.local/createJob?name=MyPipelineJob" --data-urlencode config@pipeline.yml
    - createJob?name=MyPipelineJob: API endpoint to create a new Jenkins job named MyPipelineJob.
    - config@pipeline.yml: The YAML configuration file for your Jenkins pipeline.
    - username:APITOKEN: Use your Jenkins API token for authentication.


<!-- pipeline.yml -->
jobs:
  - job:
      name: "MyPipelineJob"
      description: "Pipeline for Docker Image Build and Deployments"
      type: "Pipeline"
      definition:
        cps:
          definition:
            script: |
              pipeline {
                  agent any

                  parameters {
                      choice(
                          name: 'ENVIRONMENT',
                          choices: ['staging', 'production'],
                          description: 'Choose the environment for deployment'
                      )
                  }

                  stages {
                      stage('Checkout Code') {
                          steps {
                              checkout scm
                          }
                      }

                      stage('Generate Image Tag') {
                          steps {
                              script {
                                  def now = new Date()
                                  def formattedDate = now.format("yyyyMMdd-HHmmss")
                                  env.IMAGE_TAG = formattedDate
                              }
                          }
                      }
                  }

                  post {
                      always {
                          cleanWs()
                      }
                  }
              }
            sandbox: false
