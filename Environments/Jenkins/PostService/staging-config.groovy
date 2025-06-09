return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-postservice',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/PostService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/PostService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/PostService/Kubernetes/deployments-services/*.yaml',

    // SQL Server Configuration
    MYSQL_DEPLOYMENT_NAME: 'staging-post-deployment-mysql',
    MYSQL_CONTAINER_NAME: 'mssql-server',
    MYSQL_IMAGE: 'mysql:latest',
    MYSQL_PORT: 3306,
    MYSQL_REPLICAS: 1,
    MYSQL_SECRET_NAME: 'post-secret-credential',
    MYSQL_DEFAULT_DATABASE: 'UserDatabase',
    MYSQL_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    MYSQL_MEMORY_REQUEST: "500m",
    MYSQL_CPU_REQUEST: "512Mi",
    MYSQL_MEMORY_LIMIT: "1000m",
    MYSQL_CPU_LIMIT: "1024Mi",
    MYSQL_SERVICE_NAME: 'staging-post-service-mysql',

    // User Service Configuration
    POST_DEPLOYMENT_NAME: 'staging-post-deployment-post-service',
    POST_CONTAINER_NAME: 'userservice',
    POST_PORT: 80,
    POST_REPLICAS: 1,
    POST_ENVIRONMENT: 'Staging',
    POST_DATABASE: 'PostDatabase',
    POST_SECRET_NAME: 'post-secret-credential',
    POST_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    POST_SERVICE_NAME: 'staging-post-service-post-service',
    POST_NODE_PORT: 31010,
    POST_MEMORY_REQUEST: "500m",
    POST_CPU_REQUEST: "512Mi",
    POST_MEMORY_LIMIT: "1000m",
    POST_CPU_LIMIT: "1024Mi",
]