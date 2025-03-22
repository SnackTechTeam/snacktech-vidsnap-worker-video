namespace core.domain.dtos.messages
{
    public record DlqMessageDto
    {
        public string MensagemOriginal {get; set;} = default!;
        public ExceptionResponse ErroDeProcessamento {get; set;} = default!;
    }
}