return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-ui',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/fe/Containerizations/FakebookUI.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/UIService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/UIService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    UI_DEPLOYMENT_NAME: 'production-fakebook-ui-deployment-ui-service',
    UI_SERVICE_NAME: 'production-fakebook-ui-service-ui-service',
    UI_CONTAINER_NAME: 'uiservice',
    UI_PORT: 80,
    UI_REPLICAS: 1,
    UI_ENVIRONMENT: 'production',
    UI_NODE_PORT: 32200,
]