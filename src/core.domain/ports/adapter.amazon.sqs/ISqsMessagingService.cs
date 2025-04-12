using Amazon.SQS.Model;
using core.domain.common;

namespace core.domain.ports.adapter.amazon.sqs
{
    public interface ISqsMessagingService
    {
        Task<Result> EnviarMensagemAsync<T>(T mensagem, string urlFila, string? messageGroupId = null, string? messageDeduplicationId = null);
        Task<Result<ReceiveMessageResponse>> ConsumirMensagemDeFilaAsync(string urlFila);
        Task<Result> DeletarMensagemAsync(string urlFila, Message mensagem);
    }
}