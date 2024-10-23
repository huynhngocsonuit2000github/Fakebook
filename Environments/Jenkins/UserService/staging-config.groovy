return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-userservice',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/UserService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/UserService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/UserService/Kubernetes/deployments-services/*.yaml',

    // SQL Server Configuration
    SQL_DEPLOYMENT_NAME: 'staging-user-deployment-sqlserver',
    SQL_CONTAINER_NAME: 'mssql-server',
    SQL_IMAGE: 'mcr.microsoft.com/mssql/server:2022-latest',
    SQL_PORT: 1433,
    SQL_REPLICAS: 1,
    SQL_SECRET_NAME: 'user-secret-credential',
    SQL_SECRET_PASSWORD_KEY: 'SA_PASSWORD',
    SQL_MEMORY_REQUEST: "2048Mi",
    SQL_CPU_REQUEST: "500m",
    SQL_MEMORY_LIMIT: "4096Mi",
    SQL_CPU_LIMIT: "1",
    SQL_SERVICE_NAME: 'staging-user-service-sqlserver',

    // User Service Configuration
    USER_DEPLOYMENT_NAME: 'staging-user-deployment-user-service',
    USER_CONTAINER_NAME: 'userservice',
    USER_PORT: 80,
    USER_REPLICAS: 1,
    USER_ENVIRONMENT: 'Development',
    USER_DATABASE: 'UserDatabase',
    USER_SECRET_NAME: 'user-secret-credential',
    USER_SECRET_PASSWORD_KEY: 'SA_PASSWORD',
    USER_SERVICE_NAME: 'staging-user-service-user-service',
    USER_NODE_PORT: 31000,
]
