using core.domain.ports.adapter.amazon.sqs;

namespace adapter.api.Workers
{
    public class WorkerConsumerSqsVideoMessage : BackgroundService
    {
        private readonly ILogger<WorkerConsumerSqsVideoMessage> logger;
        private readonly ISqsMessagingService sqsMessagingService;
        public WorkerConsumerSqsVideoMessage(ILogger<WorkerConsumerSqsVideoMessage> logger, ISqsMessagingService sqsMessagingService){
            this.logger = logger;
            this.sqsMessagingService = sqsMessagingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested){
                var response = await sqsMessagingService.ConsumirMensagemDeFilaAsync("");

                if(response.IsSuccess()){
                    foreach(var message in response.Data.Messages){
                        //TODO: Serviço de processamento da mensagem

                        await sqsMessagingService.DeletarMensagemAsync("",message);
                    }
                }

                logger.LogInformation("Aguardando para consumir novas mensagens...");
                await Task.Delay(5000);
            }
        }
    }
}