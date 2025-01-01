http://localhost:8081/job/FB001-Test_internal_user_login/buildWithParameters?token=testTriggerToken

docker run -d \
--name aio-jenkins-agent-new \
-p 8082:8080 \
-p 50002:50000 \
--user root \
-v jenkins_home_aio:/var/jenkins_home \
jenkins/jenkins:lts

docker exec -it aio-jenkins-agent-new bash

========

# Call to trigger

<!-- curl -u admin:117406bbd24b3f9fb0d9477247049b272a -X POST http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/buildWithParameters?token=testTriggerToken -->

curl -u admin:117406bbd24b3f9fb0d9477247049b272a -X POST http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/build?token=testTriggerToken

curl -u admin:117406bbd24b3f9fb0d9477247049b272a -O http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/lastSuccessfulBuild/artifact/archivedReports/

curl -u admin:117406bbd24b3f9fb0d9477247049b272a -O http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/lastSuccessfulBuild/artifact/archivedReports/\_a5e8d98a3d7b_2025-01-01_15_32_39.trx

curl -u admin:117406bbd24b3f9fb0d9477247049b272a -O http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/lastSuccessfulBuild/artifact/archivedReports/final_report.txt
