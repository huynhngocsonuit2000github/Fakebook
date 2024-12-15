return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-idpapi',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/IdPService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/IdPApiService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/IdPApiService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    IDP_API_DEPLOYMENT_NAME: 'staging-fakebook-idp-deployment-api-service',
    IDP_API_SERVICE_NAME: 'staging-fakebook-idp-service-api-service',
    IDP_API_CONTAINER_NAME: 'idpapiservice',
    IDP_API_PORT: 80,
    IDP_API_REPLICAS: 1,
    IDP_API_NODE_PORT: 31001,
    IDP_API_MEMORY_REQUEST: "250m",
    IDP_API_CPU_REQUEST: "128Mi",
    IDP_API_MEMORY_LIMIT: "500m",
    IDP_API_CPU_LIMIT: "256Mi",
]