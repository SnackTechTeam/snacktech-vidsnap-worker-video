namespace core.domain.dtos
{
    public class ExceptionResponse{
        public string? Type {get; private set;}
        public string? Stack {get; private set;}
        public string? TargetSite {get; private set;}

        public ExceptionResponse(Exception exception){
            Type = exception.GetType().FullName;
            Stack = exception.StackTrace;
            TargetSite = exception.TargetSite?.ToString();
        }
    }
}