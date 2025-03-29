using core.domain.dtos;
using core.domain.dtos.messages;
using core.domain.enums;
using core.domain.models;
using core.domain.options;
using core.domain.ports.adapter.amazon.sqs;
using core.domain.ports.core.application;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace adapter.api.Workers
{
    public class WorkerConsumerSqsVideoMessage : BackgroundService
    {
        private readonly ILogger<WorkerConsumerSqsVideoMessage> logger;
        private readonly SqsOptions sqsOptions;
        private readonly ISqsMessagingService sqsMessagingService;
        private readonly IVideoMessageHandler videoMessageHandler;
        public WorkerConsumerSqsVideoMessage(ILogger<WorkerConsumerSqsVideoMessage> logger,
                                                IOptions<SqsOptions> sqsOptions, 
                                                ISqsMessagingService sqsMessagingService,
                                                IVideoMessageHandler videoMessageHandler){
            this.logger = logger;
            this.sqsOptions = sqsOptions.Value;
            this.sqsMessagingService = sqsMessagingService;
            this.videoMessageHandler = videoMessageHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested){
                var response = await sqsMessagingService.ConsumirMensagemDeFilaAsync(sqsOptions.QueueUrlConsuming);

                if(response.IsSuccess()){
                    foreach(var message in response.Data.Messages){
                       try{
                            logger.LogInformation($"Processando mensagem id {message.MessageId} - receipt {message.ReceiptHandle}");
                            NewVideoDto mensagemDeserializada = JsonConvert.DeserializeObject<NewVideoDto>(message.Body) 
                                        ?? throw new InvalidCastException($"Erro ao desserializar mensagem para NewVideoDto");

                            if(mensagemDeserializada is null || mensagemDeserializada.Records is null || !mensagemDeserializada.Records.Any())
                            {
                                logger.LogInformation($"Mensagem {message.MessageId} fora do padrão esperado ou sem registros");
                                continue;
                            }
                            await ProcessarVideo(mensagemDeserializada);
                            
                       }
                       catch(Exception ex){
                            logger.LogError(ex, $"Erro durante processamento da mensagem {message.MessageId} - {ex.Message}");
                            var dlqMessage = new DlqMessageDto{
                                MensagemOriginal = message.Body,
                                ErroDeProcessamento = new ExceptionResponse(ex)
                            };

                            await sqsMessagingService.EnviarMensagemAsync(JsonConvert.SerializeObject(dlqMessage),sqsOptions.QueueUrlDlq);
                            
                       }
                       finally{
                            await sqsMessagingService.DeletarMensagemAsync(sqsOptions.QueueUrlConsuming,message);
                       }
                    }
                }

                logger.LogInformation("Aguardando para consumir novas mensagens...");
                await Task.Delay(5000);
            }
        }

        private async Task ProcessarVideo(NewVideoDto mensagemDeserializada){
            VideoParaBaixar videoParaBaixar = new VideoParaBaixar(mensagemDeserializada);
            try{
                await EnviarMensagemInicioProcesso(videoParaBaixar);
                var result = await videoMessageHandler.ProcessVideoMessage(videoParaBaixar);

                if(!result.IsSuccess())
                {
                    await EnviarMensagemFalha(videoParaBaixar);
                    return;
                }

                await sqsMessagingService.EnviarMensagemAsync(result.Data,sqsOptions.QueueUrlProcess);
            }
            catch(Exception ex){
                logger.LogError(ex, $"Erro durante processamento de video {videoParaBaixar.CaminhoChave} - {ex.Message}");
                await EnviarMensagemFalha(videoParaBaixar);
            }
        }

        private async Task EnviarMensagemFalha(VideoParaBaixar videoParaBaixar){
            var failResultMessage = VideoProcessingStatusDto.CriarParaFalha(videoParaBaixar.CaminhoChave);
            await sqsMessagingService.EnviarMensagemAsync(failResultMessage,sqsOptions.QueueUrlProcess);
        }

        private async Task EnviarMensagemInicioProcesso(VideoParaBaixar videoParaBaixar){
            var failResultMessage = VideoProcessingStatusDto.CriarParaInicioProcesso(videoParaBaixar.CaminhoChave);
            await sqsMessagingService.EnviarMensagemAsync(failResultMessage,sqsOptions.QueueUrlProcess);
        }
    }
}