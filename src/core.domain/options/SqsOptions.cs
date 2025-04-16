
using System.Diagnostics.CodeAnalysis;

namespace core.domain.options
{
    [ExcludeFromCodeCoverage]
    public class SqsOptions
    {
        public string ServiceUrl {get; set;} = default!;
        public string QueueUrlConsuming {get; set;} = default!;
        public string QueueUrlProcess {get; set;} = default!;
        public string QueueUrlDlq {get; set;} = default!;
    }
}