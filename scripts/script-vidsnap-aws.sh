#!/bin/bash

AWS_REGION="us-east-1"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CAMINHO_POLICY="$SCRIPT_DIR/sqs-policy.json"
CAMINHO_CONFIG="file://$SCRIPT_DIR/s3-notification.json"

POLICY=$(jq -c . $CAMINHO_POLICY | sed 's/"/\\"/g')

aws s3api create-bucket --bucket bucket-videos --region "$AWS_REGION"

aws sqs create-queue --queue-name evento-novo-video

aws sqs create-queue --queue-name evento-novo-video-dlq

aws sqs create-queue --queue-name evento-video-processado

QUEUE_URL=$(aws sqs get-queue-url --queue-name evento-novo-video --query "QueueUrl" --output text)

aws sqs set-queue-attributes --queue-url "$QUEUE_URL" --attributes "{\"Policy\": \"$POLICY\"}"

aws s3api put-bucket-notification-configuration --bucket bucket-videos --notification-configuration $CAMINHO_CONFIG
