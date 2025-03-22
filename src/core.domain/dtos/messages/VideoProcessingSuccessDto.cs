namespace core.domain.dtos.messages
{
    public record VideoProcessingSuccessDto
    {
        public string ObjectKey {get; set;} = default!;
        public string ZipPath {get; set;} = default!;
        public string ImagePath {get; set;} = default!;
    }
}