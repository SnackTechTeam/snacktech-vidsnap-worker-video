using System.Diagnostics.CodeAnalysis;

namespace core.domain.options
{
    [ExcludeFromCodeCoverage]
    public class S3Options
    {
        public string ServiceUrl {get; set;} = default!;
    }
}