
using core.domain.common;
using core.domain.dtos.messages;

namespace core.domain.ports.core.application
{
    public interface IVideoMessageHandler
    {
        Task<Result<VideoProcessingSuccessDto>> ProcessVideoMessage(NewVideoDto newVideoDto);
        
    }
}