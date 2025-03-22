namespace core.domain.options
{
    public class AmazonOptions
    {
        public string Region {get; set;} = default!;
        public string AwsAccessKeyId {get; set;} = default!;
        public string AwsSecretAccessKey {get; set;} = default!;
        public string AwsSecretAccessToken {get; set;} = default!;
        public bool UseLocalStack {get; set;}
    }
}