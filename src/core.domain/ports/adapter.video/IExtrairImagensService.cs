using core.domain.common;
using core.domain.models;

namespace core.domain.ports.adapter.video
{
    public interface IExtrairImagensService
    {
        Task<Result<string>> ExtrairImagensPorIntervaloAsync(VideoParaBaixar videoParaBaixar, int intervaloEmSegundos);
    }
}