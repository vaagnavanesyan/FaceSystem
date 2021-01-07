Write-Host Installing RabbitMq
minikube profile facesystem
helm repo add center https://repo.chartcenter.io
helm install rabbitmq center/bitnami/rabbitmq
helm install mssql stable/mssql-linux --set acceptEula.value=Y --set edition.value=Developer