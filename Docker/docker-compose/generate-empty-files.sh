#!/bin/bash
rm -rf data

mkdir -p data/configs
touch data/configs/appsettings.api.json
touch data/configs/appsettings.ui.json
touch data/configs/appsettings.worker.json

mkdir -p data/certs
touch data/certs/grpc.crt
touch data/certs/grpc.key
touch data/certs/grpc.pfx
touch data/certs/app.crt
touch data/certs/app.key
touch data/certs/app.pfx
touch data/certs/ui.crt
touch data/certs/ui.key
touch data/certs/ui.pfx
touch data/certs/redis_server.crt
touch data/certs/redis_server.key
touch data/certs/redis_server.pfx
touch data/certs/redis_client.crt
touch data/certs/redis_client.key
touch data/certs/redis_client.pfx
touch data/certs/rootCA.crt
touch data/certs/rootCA.key
touch data/certs/rootCA.pfx    
