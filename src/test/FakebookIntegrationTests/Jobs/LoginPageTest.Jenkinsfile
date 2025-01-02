pipeline {
    agent any

    stages {
        stage('Checkout Code') {
            steps {
                checkout scm
                script {
                    sh 'git checkout aio-prototype'
                }
            }
        }

        stage('Verify') {
            steps {
                script {
                    echo 'Current Directory:'
                    sh 'pwd'
                    echo 'Listing Files:'
                    sh 'ls -l'
                }
            }
        }

        stage('Run Tests') {
            steps {
                script {
                    sh 'cd src/test/FakebookIntegrationTests && dotnet restore'
                    sh 'cd src/test/FakebookIntegrationTests && dotnet test --logger trx'
                }
            }
        }

        stage('Publish Test Results') {
            steps {
                publishHTML([target: [
                    allowMissing: false,
                    alwaysLinkToLastBuild: true,
                    keepAll: true,
                    reportDir: 'src/test/FakebookIntegrationTests/TestResults',
                    reportFiles: '*.html',
                    reportName: 'Test Results'
                ]])
            }
        }
    }

    post {
        always {
            script {
                def sourceDir = "/var/jenkins_home/jobs/FB001-Test_internal_user_login/builds/${env.BUILD_NUMBER}/htmlreports/Test_20Results"
                def targetDir = "${env.WORKSPACE}/archivedReports"
                
                // Create the target directory and copy reports to workspace
                sh "mkdir -p ${targetDir} && cp -r ${sourceDir}/* ${targetDir}/"

                // Create the final_report.txt with all file names in the archivedReports folder
                sh "ls ${sourceDir} > ${targetDir}/final_report.txt"

                // Archive artifacts from the workspace, including the final_report.txt
                archiveArtifacts artifacts: "archivedReports/*, final_report.txt", allowEmptyArchive: true

                cleanWs()
            }
        }
        success {
            echo 'Tests passed!'
        }
        failure {
            echo 'Tests failed.'
        }
    }

}

