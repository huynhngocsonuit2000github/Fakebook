docker compose -f ./be/Services/Containerizations/docker-compose.yaml down

docker build -t huynhngocsonuit2000docker/fakebook-userservice:v001 . -f ./be/Services/Containerizations/UserService.Dockerfile

docker compose -f ./be/Services/Containerizations/docker-compose.yaml up -d
