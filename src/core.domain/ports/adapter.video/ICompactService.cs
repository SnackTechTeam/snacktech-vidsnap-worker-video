using core.domain.common;
using core.domain.models;

namespace core.domain.ports.adapter.video
{
    public interface ICompactService
    {
        Result CompactarImagensDeVideo(VideoParaBaixar videoParaBaixar);
    }
}