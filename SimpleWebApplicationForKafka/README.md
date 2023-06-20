1. Install and start minikube.
1. Enable ingress addon.
```shell
minikube addons enable ingress
```
1. Install and setup **Helm** repos for strimzi.
```shell
helm repo add strimzi https://strimzi.io/charts/
```
1. Create kafka cluster in a namespace named `kafka`.
```shell
helm install strimzi strimzi/strimzi-kafka-operator --namespace kafka
```
1. Use `kafka.yaml`, `kafka-bridge.yaml`, and `kafka-topic.yaml` to create kafka cluster and a topic named `my-topic`.
1. Download helm repo for trino in the local folder using the command below
```shell
helm repo add trino https://trinodb.github.io/charts

helm pull trino/trino --destination <folder_Name> --untar
```
1. In the new folder `<folder_Name>` in the script above, add values below in the `values.yaml`.
```yaml
additionalCatalogs: 
   kafka: |
      connector.name=kafka
      kafka.nodes=my-cluster-kafka-bootstrap.kafka:9092
      kafka.table-names=my-topic
      kafka.hide-internal-columns=false
   postgres: |
      connector.name=postgresql
      connection-url=jdbc:postgresql://pg-service.app.svc.cluster.local:5432/postgres
      connection-user=postgres
      connection-password=postgres
```
1. Start trino cluster.
```shell
   helm install trino-glue <path to values.yaml without the file name>
```
1. Start airflow
```shell
helm repo add apache-airflow https://airflow.apache.org
helm upgrade --install airflow apache-airflow/airflow --namespace airflow --create-namespace
```
1. To add Trino provider for airflow, create a Dockerfile in a separate folder with content below.
```dockerfile
FROM apache/airflow
RUN pip install --no-cache-dir apache-airflow-providers-trino
```
1. Build a new image.
```shell
# change the tag according to your need
export IMAGE_NAME=ripplejb/my-airflow-trino

docker build --pull --tag $IMAGE_NAME:1.0.0

minikube image load $IMAGE_NAME:1.0.0

helm upgrade airflow apache-airflow/airflow --namespace airflow --set images.airflow.repository=$IMAGE_NAME --set images.airflow.tag=1.0.0
```