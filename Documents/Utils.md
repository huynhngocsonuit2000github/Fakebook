<!-- Build image -->

docker build -t huynhngocsonuit2000docker/fakebook-userservice:v006 . -f ./be/Services/Containerizations/UserService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-idpapi:v002 . -f ./be/Services/Containerizations/IdPService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-ui:v005 --build-arg ENVIRONMENT=compose . -f ./fe/Containerizations/FakebookUI.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-idpui:v006 --build-arg ENVIRONMENT=staging . -f ./fe/Containerizations/FakebookIdPUI.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-apigateway:v001 . -f ./be/Services/Containerizations/ApiGatewayService.Dockerfile
docker build -t huynhngocsonuit2000docker/fakebook-authservice:v004 . -f ./be/Services/Containerizations/AuthService.Dockerfile

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

<!-- MySQL -->

- Inside container
  mysql -u root -p

docker run --name user-mysql -e MYSQL_ROOT_PASSWORD=admin1234$ -e MYSQL_DATABASE=UserDatabase -p 3306:3306 --network docker_network -d mysql:latest
docker run --name auth-mysql -e MYSQL_ROOT_PASSWORD=admin1234$ -e MYSQL_DATABASE=AuthDatabase -p 3307:3306 --network docker_network -d mysql:latest
