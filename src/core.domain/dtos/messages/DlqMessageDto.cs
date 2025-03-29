using System.Diagnostics.CodeAnalysis;

namespace core.domain.dtos.messages
{
    [ExcludeFromCodeCoverage]
    public record DlqMessageDto
    {
        public string MensagemOriginal {get; set;} = default!;
        public ExceptionResponse ErroDeProcessamento {get; set;} = default!;
    }
}