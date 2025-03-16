using core.domain.types;

namespace core.domain.models
{
    public record VideoParaProcessar
    {
        public GuidValido ClientId;
        public required StringNaoVaziaOuComEspacos Nome;
        public UrlS3Valida Url;
    }
}