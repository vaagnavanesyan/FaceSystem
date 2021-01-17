Write-Host Installing RabbitMq
minikube profile facesystem
helm repo add center https://repo.chartcenter.io
helm install rabbitmq center/bitnami/rabbitmq --set auth.username=user --set auth.password=bX1DTrlOfH
helm install mssql stable/mssql-linux --set acceptEula.value=Y --set edition.value=Developer --set sapassword=bX1DTrlOfH