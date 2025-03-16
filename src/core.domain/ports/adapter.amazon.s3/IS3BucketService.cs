using core.domain.common;

namespace core.domain.ports.adapter.amazon.s3
{
    public interface IS3BucketService
    {
        Task<Result> BaixarArquivo(string nomeBucket, string chaveS3, string caminhoLocalDestino);        
        Task<Result> SubirArquivo(string nomeBucket, string chaveS3, string caminhoLocalArquivo);
    }
}