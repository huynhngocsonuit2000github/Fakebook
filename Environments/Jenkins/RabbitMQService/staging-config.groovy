return [
    // DockerHub repository
    IMAGE_NAME: 'rabbitmq:management',
    
    // Docker & K8S configuration
    SECRET_CONFIGMAP_PATH: 'Environments/Jenkins/RabbitMQService/Kubernetes/secrets-configmap/*.yaml',
    SERVICES_PATH: 'Environments/Jenkins/RabbitMQService/Kubernetes/deployments-services/*.yaml',

    // User Service Configuration
    RABBITMQ_DEPLOYMENT_NAME: 'staging-rabbitmq-deployment-rabbitmq-service',
    RABBITMQ_CONTAINER_NAME: 'rabbitmqservice',
    RABBITMQ_PORT_ENV: "5672",
    RABBITMQ_PORT: 5672,
    RABBITMQ_PORT_UI: 15672,
    RABBITMQ_NODE_PORT: 31672,
    RABBITMQ_REPLICAS: 1,
    RABBITMQ_ENVIRONMENT: 'Staging',
    RABBITMQ_SECRET_NAME: 'staging-rabbitmq-secret-credential',
    RABBITMQ_SERVICE_NAME: 'staging-rabbitmq-service',
    RABBITMQ_SERVICE_NAME_UI: 'staging-rabbitmq-service-ui',
    RABBITMQ_SECRET_USERNAME_KEY: 'username',
    RABBITMQ_SECRET_PASSWORD_KEY: 'password',
]