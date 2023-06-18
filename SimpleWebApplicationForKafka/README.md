1. Install and start minikube.
2. Install and setup **Helm** repos for strimzi.

```bash
helm repo add strimzi https://strimzi.io/charts/
```
3. Create kafka cluster in a namespace named `kafka`.
```bash
helm install strimzi strimzi/strimzi-kafka-operator --namespace kafka
```
4. Use `kafka.yaml`, `kafka-bridge.yaml`, and `kafka-topic.yaml` to create kafka cluster and a topic named `my-topic`.
5. Download helm repo for trino in the local folder using the command below
```bash
helm repo add trino https://trinodb.github.io/charts

helm pull trino/trino --destination <folder_Name> --untar
```
6. In the new folder `<folder_Name>` in the script above, add values below in the `values.yaml`.
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
7. Start trino cluster.
   ```bash
   helm install trino-glue <path to values.yaml without the file name>
   ```
