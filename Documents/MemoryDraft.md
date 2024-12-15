Every 1.0s: kubectl get pods,svc,deployment -n staging-environment 192.168.2.18: Sun Dec 15 11:56:30 2024

NAME READY STATUS RESTARTS AGE
pod/staging-auth-deployment-auth-service-fdd9999d9-nr6m4 0/1 CrashLoopBackOff 15 (3m49s ago) 4d10h
pod/staging-auth-deployment-mysql-9ff487dcb-7czw9 0/1 ImagePullBackOff 0 4d11h
pod/staging-auth-deployment-mysql-ffd89d899-zd5rz 0/1 ImagePullBackOff 0 4d10h
pod/staging-fakebook-apigateway-deployment-service-b77fbbc64-8662j 1/1 Running 1 (57m ago) 4d21h
pod/staging-fakebook-idp-deployment-api-service-785bc77454-g66sz 1/1 Running 1 (58m ago) 5d5h
pod/staging-fakebook-idp-deployment-ui-service-6679585b4f-n7n9s 1/1 Running 1 (57m ago) 5d4h
pod/staging-fakebook-ui-deployment-ui-service-5695bb4b78-8nkbf 1/1 Running 1 (58m ago) 5d5h
pod/staging-user-deployment-mysql-5dbb885c4-7xgph 1/1 Running 1 (58m ago) 4d11h
pod/staging-user-deployment-user-service-745c9b8cb6-qdsmn 1/1 Running 10 (35m ago) 4d11h

NAME TYPE CLUSTER-IP EXTERNAL-IP PORT(S) AGE
service/staging-auth-service-auth-service NodePort 10.97.195.164 <none> 80:31005/TCP 4d11h
service/staging-auth-service-mysql ClusterIP 10.97.252.67 <none> 3306/TCP 4d11h
service/staging-fakebook-apigateway-service-service NodePort 10.109.129.91 <none> 80:31321/TCP 4d21h
service/staging-fakebook-idp-service-api-service NodePort 10.108.237.167 <none> 80:31001/TCP 5d13h
service/staging-fakebook-idp-service-ui-service NodePort 10.106.234.31 <none> 80:31201/TCP 5d4h
service/staging-fakebook-ui-service-ui-service NodePort 10.106.213.253 <none> 80:31200/TCP 6d5h
service/staging-user-service-mysql ClusterIP 10.101.81.152 <none> 3306/TCP 4d11h
service/staging-user-service-user-service NodePort 10.111.235.50 <none> 80:31000/TCP 4d11h

NAME READY UP-TO-DATE AVAILABLE AGE
deployment.apps/staging-auth-deployment-auth-service 0/1 1 0 4d11h
deployment.apps/staging-auth-deployment-mysql 0/1 1 0 4d11h
deployment.apps/staging-fakebook-apigateway-deployment-service 1/1 1 1 4d21h
deployment.apps/staging-fakebook-idp-deployment-api-service 1/1 1 1 5d13h
deployment.apps/staging-fakebook-idp-deployment-ui-service 1/1 1 1 5d4h
deployment.apps/staging-fakebook-ui-deployment-ui-service 1/1 1 1 6d5h
deployment.apps/staging-user-deployment-mysql 1/1 1 1 4d11h
deployment.apps/staging-user-deployment-user-service 1/1 1 1 4d11h

kubectl delete services --all -n staging-environment
kubectl delete deployments --all -n staging-environment

vi /etc/resolv.conf
nameserver 8.8.8.8
nameserver 8.8.4.4
nameserver 1.1.1.1
