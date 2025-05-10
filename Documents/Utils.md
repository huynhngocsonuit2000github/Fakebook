<!-- Build image -->

docker build -t huynhngocsonuit2000docker/fakebook-userservice:v007 . -f ./be/Services/Containerizations/UserService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-idpapi:v006 . -f ./be/Services/Containerizations/IdPService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-ui:v006 --build-arg ENVIRONMENT=compose . -f ./fe/Containerizations/FakebookUI.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-idpui:v006 --build-arg ENVIRONMENT=compose . -f ./fe/Containerizations/FakebookIdPUI.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-apigateway:v004 . -f ./be/Services/Containerizations/ApiGatewayService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-authservice:v007 . -f ./be/Services/Containerizations/AuthService.Dockerfile

<!-- Run docker compose -->

docker compose -f ./be/Services/Containerizations/docker-compose.yaml down
docker compose -f ./be/Services/Containerizations/docker-compose.yaml up -d

<!-- Copy file from container -->

docker cp 4789597bd8ab:/etc/nginx/conf.d/default.conf /Users/huynhngocson/Desktop

<!-- SSH ngix -->

- edit
  vi /etc/nginx/conf.d/default.conf

- test after edit
  nginx -t

- reload
  nginx -s reload
  location / {  
   root /usr/share/nginx/html;
  index index.html index.htm;
  }

root /usr/share/nginx/html;
index index.html;

location / {
try_files $uri $uri/ /index.html;
}

<!-- Dotnet server -->

- install vi
  apt update && apt install vim -y

- update key in json file
  echo '{ "key": "new-value" }' > ocelot.Compose.json

<!-- MySQL -->

- Inside container
  mysql -u root -p

docker run --name user-mysql -e MYSQL_ROOT_PASSWORD=admin1234$ -e MYSQL_DATABASE=UserDatabase -p 3306:3306 --network docker_network -d mysql:latest
docker run --name auth-mysql -e MYSQL_ROOT_PASSWORD=admin1234$ -e MYSQL_DATABASE=AuthDatabase -p 3307:3306 --network docker_network -d mysql:latest

<!-- Clean service, deployment -->

kubectl delete pods --all -n production-environment
kubectl delete services --all -n production-environment
kubectl delete deployments --all -n production-environment

<!-- Check resource -->

kubectl describe node staging-k8s-master | grep -A 15 "Capacity"
kubectl describe node staging-k8s-worker1 | grep -A 15 "Capacity"
kubectl describe node staging-k8s-worker2 | grep -A 15 "Capacity"

<!-- ssh to the kubernete service container  -->

kubectl exec -it staging-fakebook-idp-deployment-api-service-7bb5488698-kr9j5 -n staging-environment -- /bin/bash

<!-- Fix DNS resolution -->

cat /etc/resolv.conf
echo "nameserver 8.8.8.8" > /etc/resolv.conf
