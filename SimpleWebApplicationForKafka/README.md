1. Install and start minikube.
2. Install and setup **Helm** repos for strimzi.
3. Create kafka cluster in a namespace named `kafka`.
4. Use `kafka.yaml`, `kafka-bridge.yaml`, and `kafka-topic.yaml` to create kafka cluster and a topic named `my-topic`.
5. Download helm repo for trino in the local folder using the command below
   ```bash
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
   ```
7. Start trino cluster.
   ```bash
   helm install trino-glue <path to values.yaml without the file name>
   ```
