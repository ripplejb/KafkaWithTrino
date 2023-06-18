# 1. get postgres connector from 
# https://repo1.maven.org/maven2/io/debezium/debezium-connector-postgres/2.2.1.Final/debezium-connector-postgres-2.2.1.Final-plugin.tar.gz
# and unzip in folder debezium-connector-postgres

# 2. Build and push the connector in your repository.

FROM quay.io/strimzi/kafka:0.35.1-kafka-3.4.0
USER root:root
RUN mkdir -p /opt/kafka/plugins/debezium
COPY ./debezium-connector-postgres /opt/kafka/plugins/debezium
USER 1001
