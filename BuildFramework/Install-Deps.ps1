Write-Host Installing RabbitMq
minikube profile facesystem
helm repo add center https://repo.chartcenter.io
helm install rabbitmq center/bitnami/rabbitmq