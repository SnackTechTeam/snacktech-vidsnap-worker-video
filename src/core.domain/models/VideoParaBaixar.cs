using core.domain.dtos.messages;
using core.domain.types;

namespace core.domain.models
{
    public record VideoParaBaixar
    {
        public GuidValido Cliente {get; set;}
        public VideoValido NomeVideo {get; set;}
        public GuidValido IdVideo {get; set;}
        public StringNaoVaziaOuComEspacos Bucket {get; set;} = default!;
        public StringNaoVaziaOuComEspacos Chave {get; set;} = default!;
        public StringNaoVaziaOuComEspacos CaminhoChave {get; set;} = default!;
        public string DestinoLocal() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"videos",Cliente.ToString());
        public string CaminhoVideoCompleto() => Path.Combine(DestinoLocal(),NomeVideo);
        public string DestinoImagens() => Path.Combine(DestinoLocal(),"imagens");
        public string DestinoZip() => Path.Combine(DestinoLocal(),"zip");
        public string NomeZip() => $"{Path.GetFileNameWithoutExtension(NomeVideo)}-images.zip";
        public string CaminhoZipCompleto() => Path.Combine(DestinoZip(),NomeZip());

        public VideoParaBaixar(NewVideoDto newVideoDto){
            var record = newVideoDto.Records.First();
            var objectKeyArray = record.S3.Object.Key.Split("/");
            Cliente = objectKeyArray.First();
            NomeVideo = objectKeyArray.Last();
            Bucket = record.S3.Bucket.Name;
            Chave = record.S3.Object.Key;
            CaminhoChave = Path.Combine(objectKeyArray.Take(objectKeyArray.Length - 1).ToArray());
            IdVideo = objectKeyArray.ElementAt(objectKeyArray.Length - 2);
        }
    }
}