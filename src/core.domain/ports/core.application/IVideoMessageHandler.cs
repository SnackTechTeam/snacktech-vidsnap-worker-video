
using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;

namespace core.domain.ports.core.application
{
    public interface IVideoMessageHandler
    {
        Task<Result<VideoProcessingStatusDto>> ProcessVideoMessage(VideoParaBaixar videoParaBaixar);
        
    }
}