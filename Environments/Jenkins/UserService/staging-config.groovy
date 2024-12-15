return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-userservice',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/UserService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/UserService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/UserService/Kubernetes/deployments-services/*.yaml',

    // SQL Server Configuration
    MYSQL_DEPLOYMENT_NAME: 'staging-user-deployment-mysql',
    MYSQL_CONTAINER_NAME: 'mssql-server',
    MYSQL_IMAGE: 'mysql:latest',
    MYSQL_PORT: 3306,
    MYSQL_REPLICAS: 1,
    MYSQL_SECRET_NAME: 'user-secret-credential',
    MYSQL_DEFAULT_DATABASE: 'UserDatabase',
    MYSQL_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    MYSQL_MEMORY_REQUEST: "200m",
    MYSQL_CPU_REQUEST: "256Mi",
    MYSQL_MEMORY_LIMIT: "500m",
    MYSQL_CPU_LIMIT: "512Mi",
    MYSQL_SERVICE_NAME: 'staging-user-service-mysql',

    // User Service Configuration
    USER_DEPLOYMENT_NAME: 'staging-user-deployment-user-service',
    USER_CONTAINER_NAME: 'userservice',
    USER_PORT: 80,
    USER_REPLICAS: 1,
    USER_ENVIRONMENT: 'Staging',
    USER_DATABASE: 'UserDatabase',
    USER_SECRET_NAME: 'user-secret-credential',
    USER_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    USER_SERVICE_NAME: 'staging-user-service-user-service',
    USER_NODE_PORT: 31000,
]