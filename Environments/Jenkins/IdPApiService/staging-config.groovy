return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-idpapi',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/IdPService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/IdPApiService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/IdPApiService/Kubernetes/deployments-services/*.yaml',

    // SQL Server Configuration
    MYSQL_DEPLOYMENT_NAME: 'staging-fakebook-idp-deployment-mysql',
    MYSQL_CONTAINER_NAME: 'mssql-server',
    MYSQL_IMAGE: 'mysql:latest',
    MYSQL_PORT: 3306,
    MYSQL_REPLICAS: 1,
    MYSQL_SECRET_NAME: 'idp-secret-private-key',
    MYSQL_DEFAULT_DATABASE: 'IdPDatabase',
    MYSQL_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
    MYSQL_MEMORY_REQUEST: "500m",
    MYSQL_CPU_REQUEST: "512Mi",
    MYSQL_MEMORY_LIMIT: "1000m",
    MYSQL_CPU_LIMIT: "1024Mi",
    MYSQL_SERVICE_NAME: 'staging-fakebook-idp-service-mysql',

    // User Service Configuration
    IDP_API_DEPLOYMENT_NAME: 'staging-fakebook-idp-deployment-api-service',
    IDP_API_SERVICE_NAME: 'staging-fakebook-idp-service-api-service',
    IDP_API_CONTAINER_NAME: 'idpapiservice',
    IDP_API_VOLUME_NAME: "private-key-volume",
    IDP_API_KEYS: "idp-secret-private-key",
    IDP_API_MAP_PATH: "/app/keys",
    IDP_API_PORT: 80,
    IDP_API_REPLICAS: 1,
    IDP_API_NODE_PORT: 31001,
    IDP_API_MEMORY_REQUEST: "250m",
    IDP_API_CPU_REQUEST: "128Mi",
    IDP_API_MEMORY_LIMIT: "500m",
    IDP_API_CPU_LIMIT: "256Mi",
    IDP_API_ENVIRONMENT: 'Staging',
    IDP_API_DATABASE: 'IdPDatabase',
    IDP_API_SECRET_NAME: 'idp-secret-private-key',
    IDP_API_SECRET_PASSWORD_KEY: 'ROOT_PASSWORD',
]