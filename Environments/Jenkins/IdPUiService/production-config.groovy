return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-idpui',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/fe/Containerizations/FakebookIdPUI.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/IdPUiService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/IdPUiService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    IDP_UI_DEPLOYMENT_NAME: 'production-fakebook-idp-deployment-ui-service',
    IDP_UI_SERVICE_NAME: 'production-fakebook-idp-service-ui-service',
    IDP_UI_CONTAINER_NAME: 'idpuiservice',
    IDP_UI_PORT: 80,
    IDP_UI_REPLICAS: 1,
    IDP_UI_NODE_PORT: 32201,
]