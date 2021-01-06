Write-Host Creating Cluster
minikube start -p facesystem
minikube profile facesystem
Write-Host Installing RabbitMq
helm repo add center https://repo.chartcenter.io
helm install rabbitmq center/bitnami/rabbitmq
Write-Host Starting Dashboard
minikube dashboard