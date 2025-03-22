
namespace core.domain.options
{
    public class SqsOptions
    {
        public string ServiceUrl {get; set;} = default!;
        public string QueueUrlConsuming {get; set;} = default!;
        public string QueueUrlProducing {get; set;} = default!;
        public string QueueUrlProcessSuccess {get; set;} = default!;
        public string QueueUrlDlq {get; set;} = default!;
    }
}