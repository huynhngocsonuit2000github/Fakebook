return [
    // DockerHub repository
    IMAGE_NAME: 'huynhngocsonuit2000docker/fakebook-idpui',
    
    // Docker & K8S configuration
    DOCKERFILE: 'src/fe/Containerizations/FakebookIdPUI.Dockerfile',
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/IdPUiService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/IdPUiService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    IDP_UI_DEPLOYMENT_NAME: 'staging-fakebook-idp-deployment-ui-service',
    IDP_UI_SERVICE_NAME: 'staging-fakebook-idp-service-ui-service',
    IDP_UI_CONTAINER_NAME: 'idpuiservice',
    IDP_UI_PORT: 80,
    IDP_UI_REPLICAS: 1,
    IDP_UI_NODE_PORT: 31201,
    IDP_UI_MEMORY_REQUEST: "50m",
    IDP_UI_CPU_REQUEST: "50Mi",
    IDP_UI_MEMORY_LIMIT: "100m",
    IDP_UI_CPU_LIMIT: "100Mi",
]