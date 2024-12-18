return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-authservice',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/AuthService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/AuthService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/AuthService/Kubernetes/deployments-services/*.yaml',

    // SQL Server Configuration
    MYSQL_DEPLOYMENT_NAME: 'production-auth-deployment-mysql',
    MYSQL_CONTAINER_NAME: 'mssql-server',
    MYSQL_IMAGE: 'mysql:latest',
    MYSQL_PORT: 3306,
    MYSQL_REPLICAS: 1,
    MYSQL_SECRET_NAME: 'auth-secret-credential',
    MYSQL_DEFAULT_DATABASE: 'AuthDatabase',
    MYSQL_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    MYSQL_MEMORY_REQUEST: "200m",
    MYSQL_CPU_REQUEST: "256Mi",
    MYSQL_MEMORY_LIMIT: "500m",
    MYSQL_CPU_LIMIT: "512Mi",
    MYSQL_SERVICE_NAME: 'production-auth-service-mysql',

    // User Service Configuration
    AUTH_DEPLOYMENT_NAME: 'production-auth-deployment-auth-service',
    AUTH_CONTAINER_NAME: 'userservice',
    AUTH_VOLUME_NAME: "public-key-volume",
    AUTH_MAP_PATH: "/app/keys",
    AUTH_PORT: 80,
    AUTH_REPLICAS: 1,
    AUTH_ENVIRONMENT: 'Production',
    AUTH_DATABASE: 'AuthDatabase',
    AUTH_SECRET_NAME: 'auth-secret-credential',
    AUTH_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    AUTH_SERVICE_NAME: 'production-auth-service-auth-service',
    AUTH_NODE_PORT: 32005,
    AUTH_MEMORY_REQUEST: "200m",
    AUTH_CPU_REQUEST: "128Mi",
    AUTH_MEMORY_LIMIT: "500m",
    AUTH_CPU_LIMIT: "256Mi",
]