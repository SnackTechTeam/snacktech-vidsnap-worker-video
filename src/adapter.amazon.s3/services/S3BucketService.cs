using Amazon.S3;
using Amazon.S3.Model;
using core.domain.common;
using core.domain.models;
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

        public async Task<Result> BaixarArquivoAsync(VideoParaBaixar videoParaBaixar){
            try{
                logger.LogInformation($"Baixando {videoParaBaixar.Chave} localmente...");
                Directory.CreateDirectory(videoParaBaixar.DestinoLocal());
                string fileLocalPath = videoParaBaixar.CaminhoVideoCompleto();

                var getRequest = new GetObjectRequest{
                    BucketName = videoParaBaixar.Bucket,
                    Key = videoParaBaixar.Chave
                };

                using var response = await s3Client.GetObjectAsync(getRequest);
                await using var responseStream = response.ResponseStream;
                await using var fileStream = File.Create(fileLocalPath);

                await responseStream.CopyToAsync(fileStream);

                logger.LogInformation($"Download de {videoParaBaixar.Chave} concluído. Salvo em {fileLocalPath}");
                return new Result();
            }
            catch(Exception exception){
                logger.LogError(exception,$"Erro - BaixarArquivo - {exception.Message}");
                return new Result(exception);
            }
        }

        public async Task<Result> SubirArquivoAsync(string nomeBucket, string chaveS3, string caminhoLocalArquivo){
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