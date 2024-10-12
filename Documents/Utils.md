<!-- Build image -->
docker build -t fakebook.userservice:v001 . -f ./be/Services/Containerizations/UserService.Dockerfile

<!-- Run docker compose -->
docker compose -f ./be/Services/Containerizations/docker-compose.yaml up -d