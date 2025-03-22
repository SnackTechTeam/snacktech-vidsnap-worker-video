namespace core.domain.options
{
    public class AmazonOptions
    {
        public string AwsAccessKeyId {get; set;} = default!;
        public string AwsSecretAccessKey {get; set;} = default!;
        public string AwsSecretAccessToken {get; set;} = default!;
    }
}