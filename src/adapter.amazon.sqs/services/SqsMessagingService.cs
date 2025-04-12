using Amazon.SQS;
using Amazon.SQS.Model;
using core.domain.common;
using core.domain.ports.adapter.amazon.sqs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace adapter.amazon.sqs.services
{
    public class SqsMessagingService: ISqsMessagingService
    {
        private readonly ILogger<SqsMessagingService> logger;
        private readonly IAmazonSQS sqsClient;

        public SqsMessagingService(ILogger<SqsMessagingService> logger,IAmazonSQS sqsClient){
            this.logger = logger;
            this.sqsClient = sqsClient;
        }

        public async Task<Result> EnviarMensagemAsync<T>(
                T mensagem, 
                string urlFila, 
                string? messageGroupId = null,
                string? messageDeduplicationId = null)
        {
            bool isFifoQueue = urlFila.EndsWith(".fifo", StringComparison.OrdinalIgnoreCase);
            if (isFifoQueue && string.IsNullOrWhiteSpace(messageGroupId))
            {
                var errorMsg = "MessageGroupId is required for FIFO queues.";
                logger.LogError(errorMsg + " Queue URL: {QueueUrl}", urlFila);
                return new Result(new ArgumentException(errorMsg, nameof(messageGroupId)));
            }
            try{
                var mensagemSerializada = JsonConvert.SerializeObject(mensagem);

                var request = new SendMessageRequest{
                    QueueUrl = urlFila,
                    MessageBody = mensagemSerializada
                };

                if (isFifoQueue)
                {
                    request.MessageGroupId = messageGroupId;

                    if (!string.IsNullOrWhiteSpace(messageDeduplicationId))
                    {
                        request.MessageDeduplicationId = messageDeduplicationId;
                    }
                }

                await sqsClient.SendMessageAsync(request);
                return new Result();
            }
            catch(Exception ex){
                logger.LogError(ex,$"Erro ao enviar mensagem para {urlFila} - {ex.Message}");
                return new Result(ex);
            }
        }

        public async Task<Result<ReceiveMessageResponse>> ConsumirMensagemDeFilaAsync(string urlFila){
            try{
                var request = new ReceiveMessageRequest{
                    QueueUrl = urlFila,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 5
                };
                var resultado = await sqsClient.ReceiveMessageAsync(request);
                return new Result<ReceiveMessageResponse>(resultado);
            }
            catch(Exception ex){
                logger.LogError(ex,$"Ero ao consumir mensagem de {urlFila} - {ex.Message}");
                return new Result<ReceiveMessageResponse>(ex);
            }
        }

        public async Task<Result> DeletarMensagemAsync(string urlFila, Message mensagem){
            try{
                var request = new DeleteMessageRequest{
                    QueueUrl = urlFila,
                    ReceiptHandle = mensagem.ReceiptHandle
                };

                await sqsClient.DeleteMessageAsync(request);

                return new Result();
            }
            catch(Exception ex){
                logger.LogError(ex,$"Erro ao deletar mensagem {urlFila} - {ex.Message}");
                return new Result(ex);
            }
        }
    }
}