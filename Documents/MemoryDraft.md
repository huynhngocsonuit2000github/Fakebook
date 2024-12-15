# User My SQL

compose-user-mysql:
image: mysql:latest
container_name: compose-user-mysql
environment: - MYSQL_ROOT_PASSWORD=admin1234$ - MYSQL_DATABASE=UserDatabase
ports: - "3306:3306"
networks: - docker_compose_network
volumes: - user-mysql-data:/var/lib/mysql
command: --default-authentication-plugin=mysql_native_password # Disable SSL

# User service

compose-user-user-service:
image: huynhngocsonuit2000docker/fakebook-userservice:v005
container_name: compose-user-user-service
environment: - ASPNETCORE_ENVIRONMENT=Compose - ConnectionStrings\_\_DefaultConnection=Server=compose-user-mysql;Port=3306;Database=UserDatabase;User=root;Password=$Password;SslMode=None;AllowPublicKeyRetrieval=True;
ports: - "4000:80"
depends_on: - compose-user-mysql
networks: - docker_compose_network
