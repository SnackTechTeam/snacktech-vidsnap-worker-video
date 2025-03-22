using core.domain.types;

namespace core.domain.models
{
    public record VideoParaBaixar
    {
        public GuidValido Cliente {get; set;}
        public VideoValido NomeVideo {get; set;}
        public StringNaoVaziaOuComEspacos Bucket {get; set;} = default!;
        public StringNaoVaziaOuComEspacos Chave {get; set;} = default!;
        public StringNaoVaziaOuComEspacos CaminhoChave {get; set;} = default!;
        public string DestinoLocal() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"videos",Cliente.ToString());
        public string CaminhoVideoCompleto() => Path.Combine(DestinoLocal(),NomeVideo);
        public string DestinoImagens() => Path.Combine(DestinoLocal(),"imagens");
        public string DestinoZip() => Path.Combine(DestinoLocal(),"zip");
        public string NomeZip() => $"{Cliente}-{NomeVideo}.zip";
        public string CaminhoZipCompleto() => Path.Combine(DestinoZip(),NomeZip());
    }
}