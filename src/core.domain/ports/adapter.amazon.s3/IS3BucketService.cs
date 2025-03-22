using core.domain.common;
using core.domain.models;

namespace core.domain.ports.adapter.amazon.s3
{
    public interface IS3BucketService
    {
        Task<Result> BaixarArquivoAsync(VideoParaBaixar videoParaBaixar);      
        Task<Result> SubirArquivoAsync(string nomeBucket, string chaveS3, string caminhoLocalArquivo);
    }
}