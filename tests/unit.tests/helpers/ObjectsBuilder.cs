using core.domain.dtos.messages;
using DomainDtosMessages = core.domain.dtos.messages;

namespace unit.tests.helpers
{
    public static class ObjectsBuilder
    {
        public static NewVideoDto NewVideoDtoBuilder()
            => new NewVideoDto{
                Records = new List<DomainDtosMessages.Record>{
                    new DomainDtosMessages.Record{
                        S3 = new S3{
                            Bucket = new Bucket{
                                Name = "bucket-name",
                            },
                            Object = new DomainDtosMessages.Object{
                                Key = "folder/video.mp4"
                            }
                        }
                    }
                }
            };
    }
}