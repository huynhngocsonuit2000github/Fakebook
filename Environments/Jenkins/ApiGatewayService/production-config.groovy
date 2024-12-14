return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-apigateway',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/be/Services/Containerizations/ApiGatewayService.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/ApiGatewayService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/ApiGatewayService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    API_GATEWAY_ENVIRONMENT: 'Production',
    API_GATEWAY_DEPLOYMENT_NAME: 'production-fakebook-apigateway-deployment-service',
    API_GATEWAY_SERVICE_NAME: 'production-fakebook-apigateway-service',
    API_GATEWAY_CONTAINER_NAME: 'apigatewayservice',
    API_GATEWAY_PORT: 80,
    API_GATEWAY_REPLICAS: 1,
    API_GATEWAY_NODE_PORT: 32321,
]