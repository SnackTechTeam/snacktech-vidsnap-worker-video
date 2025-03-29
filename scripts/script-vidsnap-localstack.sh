#!/bin/bash

export AWS_ENDPOINT="http://localhost:4566"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CAMINHO_POLICY="$SCRIPT_DIR/sqs-policy.json"
CAMINHO_CONFIG="file://$SCRIPT_DIR/s3-notification.json"

POLICY=$(jq -c . $CAMINHO_POLICY | sed 's/"/\\"/g')

awslocal s3api create-bucket --bucket bucket-videos --region us-east-1

awslocal sqs create-queue --queue-name evento-novo-video

awslocal sqs create-queue --queue-name evento-novo-video-dlq

awslocal sqs create-queue --queue-name evento-video-processado

awslocal sqs set-queue-attributes --queue-url http://localhost:4566/000000000000/evento-novo-video --attributes "{\"Policy\": \"$POLICY\"}"

awslocal s3api put-bucket-notification-configuration --bucket bucket-videos --notification-configuration $CAMINHO_CONFIG