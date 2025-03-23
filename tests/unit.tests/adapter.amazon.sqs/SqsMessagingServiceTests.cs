using adapter.amazon.sqs.services;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace unit.tests.adapter.amazon.sqs
{
    public class SqsMessagingServiceTests
    {
        private readonly Mock<ILogger<SqsMessagingService>> mockLogger;
        private readonly Mock<IAmazonSQS> mockSqsClient;
        private readonly SqsMessagingService service;

        public SqsMessagingServiceTests()
        {
            mockLogger = new Mock<ILogger<SqsMessagingService>>();
            mockSqsClient = new Mock<IAmazonSQS>();
            service = new SqsMessagingService(mockLogger.Object, mockSqsClient.Object);
        }

        [Fact]
        public async Task EnviarMensagemAsyncDeveRetornarSucesso()
        {
            var mensagem = new { Id = 1, Nome = "Teste" };
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";

            mockSqsClient
                .Setup(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(new SendMessageResponse());

            var result = await service.EnviarMensagemAsync(mensagem, urlFila);

            Assert.True(result.IsSuccess());
            mockSqsClient.Verify(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task EnviarMensagemAsyncDeveRetornarErroQuandoExcecaoForLancada()
        {
            var mensagem = new { Id = 1, Nome = "Teste" };
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";

            mockSqsClient
                .Setup(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ThrowsAsync(new Exception("Erro ao enviar mensagem"));

            var result = await service.EnviarMensagemAsync(mensagem, urlFila);

            Assert.False(result.IsSuccess());
            Assert.NotNull(result.Exception);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao enviar mensagem")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [Fact]
        public async Task ConsumirMensagemDeFilaAsyncDeveRetornarMensagem()
        {
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";
            var response = new ReceiveMessageResponse
            {
                Messages = new System.Collections.Generic.List<Message>
                {
                    new Message { Body = "Teste" }
                }
            };

            mockSqsClient
                .Setup(client => client.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
                .ReturnsAsync(response);

            var result = await service.ConsumirMensagemDeFilaAsync(urlFila);

            Assert.True(result.IsSuccess());
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.Messages);
            mockSqsClient.Verify(client => client.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task ConsumirMensagemDeFilaAsyncDeveRetornarErroQuandoExcecaoForLancada()
        {
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";

            mockSqsClient
                .Setup(client => client.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
                .ThrowsAsync(new Exception("Erro ao consumir mensagem"));

            var result = await service.ConsumirMensagemDeFilaAsync(urlFila);

            Assert.False(result.IsSuccess());
            Assert.NotNull(result.Exception);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao consumir mensagem")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [Fact]
        public async Task DeletarMensagemAsyncDeveRetornarSucesso()
        {
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";
            var mensagem = new Message { ReceiptHandle = "abc123" };

            mockSqsClient
                .Setup(client => client.DeleteMessageAsync(It.IsAny<DeleteMessageRequest>(), default))
                .ReturnsAsync(new DeleteMessageResponse());

            var result = await service.DeletarMensagemAsync(urlFila, mensagem);

            Assert.True(result.IsSuccess());
            mockSqsClient.Verify(client => client.DeleteMessageAsync(It.IsAny<DeleteMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task DeletarMensagemAsyncDeveRetornarErroQuandoExcecaoForLancada()
        {
            var urlFila = "https://sqs.us-east-1.amazonaws.com/123456789012/TestQueue";
            var mensagem = new Message { ReceiptHandle = "abc123" };

            mockSqsClient
                .Setup(client => client.DeleteMessageAsync(It.IsAny<DeleteMessageRequest>(), default))
                .ThrowsAsync(new Exception("Erro ao deletar mensagem"));

            var result = await service.DeletarMensagemAsync(urlFila, mensagem);

            Assert.False(result.IsSuccess());
            Assert.NotNull(result.Exception);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao deletar mensagem")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}