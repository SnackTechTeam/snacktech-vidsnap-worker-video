using adapter.api.Workers;
using Amazon.SQS.Model;
using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;
using core.domain.options;
using core.domain.ports.adapter.amazon.sqs;
using core.domain.ports.core.application;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using unit.tests.helpers;

namespace unit.tests.adapter.api.workers
{
    public class WorkerConsumerSqsVideoMessageTests
    {
        private readonly Mock<ILogger<WorkerConsumerSqsVideoMessage>> mockLogger;
        private readonly Mock<IOptions<SqsOptions>> mockOptions;
        private readonly Mock<ISqsMessagingService> mockSqsMessagingService;
        private readonly Mock<IVideoMessageHandler> mockVideoMessageHandler;
        private readonly SqsOptions sqsOptions;

        public WorkerConsumerSqsVideoMessageTests()
        {
            mockLogger = new Mock<ILogger<WorkerConsumerSqsVideoMessage>>();
            mockOptions = new Mock<IOptions<SqsOptions>>();
            mockSqsMessagingService = new Mock<ISqsMessagingService>();
            mockVideoMessageHandler = new Mock<IVideoMessageHandler>();

            sqsOptions = new SqsOptions
            {
                QueueUrlConsuming = "test-queue-url",
                QueueUrlDlq = "test-dlq-url",
                QueueUrlProcess = "test-success-url"
            };

            mockOptions.Setup(o => o.Value).Returns(sqsOptions);
        }

        [Fact]
        public async Task ExecuteAsyncDeveProcessarMessageComSucesso()
        {
            var videoDto = ObjectsBuilder.NewVideoDtoBuilder();
            var resposta = new ReceiveMessageResponse{
                Messages = new List<Message>{
                    new Message{
                        MessageId = "Id",
                        ReceiptHandle = "Receipt",
                        Body = JsonConvert.SerializeObject(videoDto)
                    }
                }
            };
            var retornoSucesso = new VideoProcessingStatusDto{

            };
            mockSqsMessagingService.Setup(s => s.ConsumirMensagemDeFilaAsync(It.IsAny<string>())).ReturnsAsync(new Result<ReceiveMessageResponse>(resposta));
            mockVideoMessageHandler.Setup(h => h.ProcessVideoMessage(It.IsAny<VideoParaBaixar>())).ReturnsAsync(new Result<VideoProcessingStatusDto>(retornoSucesso));

            var worker = new WorkerConsumerSqsVideoMessage(mockLogger.Object, mockOptions.Object, mockSqsMessagingService.Object, mockVideoMessageHandler.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            await worker.StartAsync(cancellationTokenSource.Token);

            mockVideoMessageHandler.Verify(h => h.ProcessVideoMessage(It.IsAny<VideoParaBaixar>()), Times.Once);
            mockSqsMessagingService.Verify(s => s.EnviarMensagemAsync(It.IsAny<object>(), It.IsAny<string>(),It.IsAny<string?>(),It.IsAny<string?>() ), Times.Exactly(2));
            mockSqsMessagingService.Verify(s => s.DeletarMensagemAsync(It.IsAny<string>(), It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsyncDevePularMensagensInvalidas()
        {
            var videoDto = new DlqMessageDto{

            };
            var resposta = new ReceiveMessageResponse{
                Messages = new List<Message>{
                    new Message{
                        MessageId = "Id",
                        ReceiptHandle = "Receipt",
                        Body = JsonConvert.SerializeObject(videoDto)
                    }
                }
            };
            mockSqsMessagingService.Setup(s => s.ConsumirMensagemDeFilaAsync(It.IsAny<string>())).ReturnsAsync(new Result<ReceiveMessageResponse>(resposta));

            var worker = new WorkerConsumerSqsVideoMessage(mockLogger.Object, mockOptions.Object, mockSqsMessagingService.Object, mockVideoMessageHandler.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            await worker.StartAsync(cancellationTokenSource.Token);

            mockVideoMessageHandler.Verify(h => h.ProcessVideoMessage(It.IsAny<VideoParaBaixar>()), Times.Never);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"fora do padrão esperado ou sem registros")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task ExecuteAsyncDeveEnviarMensagemParaDlqEmCasoDeFalha()
        {
            var videoDto = ObjectsBuilder.NewVideoDtoBuilderWithValues("clienteInvalido","idVideoInvalido","video.mp4");
            var resposta = new ReceiveMessageResponse{
                Messages = new List<Message>{
                    new Message{
                        MessageId = "Id",
                        ReceiptHandle = "Receipt",
                        Body = JsonConvert.SerializeObject(videoDto)
                    }
                }
            };
            
            mockSqsMessagingService.Setup(s => s.ConsumirMensagemDeFilaAsync(It.IsAny<string>())).ReturnsAsync(new Result<ReceiveMessageResponse>(resposta));
            mockVideoMessageHandler.Setup(h => h.ProcessVideoMessage(It.IsAny<VideoParaBaixar>())).ReturnsAsync(new Result<VideoProcessingStatusDto>("error",true));

            var worker = new WorkerConsumerSqsVideoMessage(mockLogger.Object, mockOptions.Object, mockSqsMessagingService.Object, mockVideoMessageHandler.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            await worker.StartAsync(cancellationTokenSource.Token);

            mockSqsMessagingService.Verify(s => s.EnviarMensagemAsync(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsyncDeveManipularExceptionsEEnviarStatusDeErro()
        {
              var videoDto = ObjectsBuilder.NewVideoDtoBuilder();
            var resposta = new ReceiveMessageResponse{
                Messages = new List<Message>{
                    new Message{
                        MessageId = "Id",
                        ReceiptHandle = "Receipt",
                        Body = JsonConvert.SerializeObject(videoDto)
                    }
                }
            };
            
            mockSqsMessagingService.Setup(s => s.ConsumirMensagemDeFilaAsync(It.IsAny<string>())).ReturnsAsync(new Result<ReceiveMessageResponse>(resposta));
            mockVideoMessageHandler.Setup(h => h.ProcessVideoMessage(It.IsAny<VideoParaBaixar>())).ThrowsAsync(new Exception("Processing error"));

            var worker = new WorkerConsumerSqsVideoMessage(mockLogger.Object, mockOptions.Object, mockSqsMessagingService.Object, mockVideoMessageHandler.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            await worker.StartAsync(cancellationTokenSource.Token);

            mockSqsMessagingService.Verify(s => s.EnviarMensagemAsync(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Exactly(2));
        }
    }
}