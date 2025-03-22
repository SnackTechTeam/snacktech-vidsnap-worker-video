using core.domain.types;

namespace core.domain.models
{
    public record VideoParaBaixar
    {
        public VideoValido NomeVideo {get; set;}
        public StringNaoVaziaOuComEspacos Bucket {get; set;} = default!;
        public StringNaoVaziaOuComEspacos Chave {get; set;} = default!;
        public string DestinoLocal()
            => $"/espaco-videos/{NomeVideo}";
    }
}