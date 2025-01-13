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

=====

# Test Jenkins Agent

testTriggerToken
admin:117406bbd24b3f9fb0d9477247049b272a

curl -u admin:117406bbd24b3f9fb0d9477247049b272a -X POST http://test-jenkins-agent-new:8080/job/FB001-Test_internal_user_login/build?token=testTriggerToken

# AIO Jenkins Agent

aio_jenkins
admin:11d731d093618b516824fbaae075ceca24
curl -u admin:11d731d093618b516824fbaae075ceca24 -X POST http://aio-jenkins-agent-new:8080/job/Trigger-Test-Agent/build?token=aio_jenkins
curl -u admin:11d731d093618b516824fbaae075ceca24 -X POST "http://localhost:8082/job/Trigger-Test-Agent/build?token=aio_jenkins"
curl -u admin:11d731d093618b516824fbaae075ceca24 -X POST "http://localhost:8082/job/Trigger-Test-Agent/buildWithParameters?token=aio_jenkins&caseId=a0a386cb-f7a8-4250-9998-e41b44747e49&jobName=FB001-Test_internal_user_login"

======

# test api get report

curl -X POST "http://localhost:5051/api/reports/trigger-job"

curl -X POST "http://localhost:5050/api/reports/upload/a0a386cb-f7a8-4250-9998-e41b44747e49" \
 -F "files=@./\_a5e8d98a3d7b_2025-01-02_14_05_48.trx" \
 -F "files=@./\_a5e8d98a3d7b_2025-01-04_04_10_49.trx" \
 -F "files=@./htmlpublisher-wrapper.html"

<!-- deploy -->

# Go to the src folder && build the image

docker build -t huynhngocsonuit2000docker/fakebook-aioservice:v001 . -f ./be/Services/Containerizations/AIOService.Dockerfile

# Pipeline database

JobName // TestJobName
JobDescription
AuthToken // to trigger job after create so_test_jenkins
PipelineContent
