using Amazon.S3;
using Amazon.S3.Model;
using core.domain.common;
using core.domain.ports.adapter.amazon.s3;
using Microsoft.Extensions.Logging;

namespace adapter.amazon.s3.services
{
    public class S3BucketService : IS3BucketService
    {
        private readonly ILogger<S3BucketService> logger;
        private readonly IAmazonS3 s3Client;
        public S3BucketService(ILogger<S3BucketService> logger, IAmazonS3 s3Client){
            this.logger = logger;
            this.s3Client = s3Client;
        }

        public async Task<Result> BaixarArquivo(string nomeBucket, string chaveS3, string caminhoLocalDestino){
            try{
                logger.LogInformation($"Baixando {chaveS3} localmente...");

                var getRequest = new GetObjectRequest{
                    BucketName = nomeBucket,
                    Key = chaveS3
                };

                using var response = await s3Client.GetObjectAsync(getRequest);
                await using var responseStream = response.ResponseStream;
                await using var fileStream = File.Create(caminhoLocalDestino);

                await responseStream.CopyToAsync(fileStream);

                logger.LogInformation($"Download de {chaveS3} concluído. Salvo em {caminhoLocalDestino}");
                return new Result();
            }
            catch(Exception exception){
                logger.LogError(exception,$"Erro - BaixarArquivo - {exception.Message}");
                return new Result(exception);
            }
        }

        public async Task<Result> SubirArquivo(string nomeBucket, string chaveS3, string caminhoLocalArquivo){
            try{
                logger.LogInformation($"Subindo {caminhoLocalArquivo} para S3...");

                var putRequest = new PutObjectRequest{
                    BucketName = nomeBucket,
                    Key = chaveS3,
                    FilePath = caminhoLocalArquivo,
                    ContentType = "application/octet-stream"
                };

                var response = await s3Client.PutObjectAsync(putRequest);
                
                logger.LogInformation($"Upload concluído. Response: {response.HttpStatusCode}");

                return new Result();
            }
            catch(Exception ex){
                return new Result(ex);
            }
        }
    }
}