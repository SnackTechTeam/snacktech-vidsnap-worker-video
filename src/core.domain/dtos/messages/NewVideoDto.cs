namespace core.domain.dtos.messages
{
    public class NewVideoDto
    {
        public required IEnumerable<Record> Records;    
    }

    public class Record{
        public string EventVersion {get; set;} = default!;
        public string EventSource {get; set;} = default!;
        public string AwsRegion {get; set;} = default!;
        public DateTime EventTime {get; set;}
        public string EventName {get; set;} = default!;
        public UserIdentity UserIdentity {get; set;} = default!;
        public RequestParameters RequestParameters {get; set;} = default!;
        public ResponseElements ResponseElements {get; set;} = default!;
        public S3 S3 {get; set;} = default!;
    }

    public class UserIdentity{
        public string PrincipalId {get; set;} = default!;
    }

    public class RequestParameters{
        public string SourceIPAddress {get; set;} = default!;
    }

    public class ResponseElements {
        public string XAmzRequestId {get; set;} = default!;
        public string XAmzId2 {get; set;} = default!;
    }

    public class S3{
        public string S3SchemaVersion {get; set;} = default!;
        public string ConfigurationId {get; set;} = default!;
        public Bucket Bucket {get; set;} = default!;
        public Object Object {get; set;} = default!;
    }

    public class Bucket{
        public string Name {get; set;} = default!;
        public OwnerIdentity OwnerIdentity {get; set;} = default!;
        public string Arn {get; set;} = default!;
    }

    public class OwnerIdentity{
        public string PrincipalId {get; set;} = default!;
    }

    public class Object{
        public string Key {get; set;} = default!;
        public string Sequencer {get; set;} = default!;
        public string eTag {get; set;} = default!;
        public Int64 Size {get; set;}
    }

}