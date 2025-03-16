using core.domain.common;

namespace core.domain.ports.adapter.video
{
    public interface IExtrairImagensService
    {
        Task<Result> ExtrairImagensPorIntervaloAsync(string nomeArquivo, string caminhoLocal, string caminhoDestino, int intervaloEmSegundos);
    }
}