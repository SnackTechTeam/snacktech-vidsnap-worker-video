using core.domain.types;

namespace core.domain.models
{
    public record VideoParaBaixar
    {
        public VideoValido NomeVideo {get; set;}
        public StringNaoVaziaOuComEspacos Bucket {get; set;} = default!;
        public StringNaoVaziaOuComEspacos Chave {get; set;} = default!;
        public readonly string DestinoLocal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"espaco-videos");
        public string DestinoCompleto() => Path.Combine(DestinoLocal,NomeVideo);
    }
}