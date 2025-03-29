using core.domain.dtos.messages;
using core.domain.models;
using DomainDtosMessages = core.domain.dtos.messages;

namespace unit.tests.helpers
{
    public static class ObjectsBuilder
    {
        public static NewVideoDto NewVideoDtoBuilder()
            => NewVideoDtoBuilderWithValues(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),"video.mp4");

        public static NewVideoDto NewVideoDtoBuilderWithValues(string cliente,string idVideo, string nomeVideo)
            => new NewVideoDto{
                Records = new List<DomainDtosMessages.Record>{
                    new DomainDtosMessages.Record{
                        S3 = new S3{
                            Bucket = new Bucket{
                                Name = "bucket-name",
                            },
                            Object = new DomainDtosMessages.Object{
                                Key = $"{cliente}/{idVideo}/{nomeVideo}"
                            }
                        }
                    }
                }
            };

        public static VideoParaBaixar VideoParaBaixarBuilder()
        => new VideoParaBaixar(NewVideoDtoBuilder());

        public static VideoParaBaixar VideoParaBaixarBuilderWithValues(string cliente,string idVideo, string nomeVideo)
        => new VideoParaBaixar(NewVideoDtoBuilderWithValues(cliente,idVideo,nomeVideo));
    }
}