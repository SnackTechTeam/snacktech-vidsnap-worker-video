namespace core.domain.types
{
    public struct VideoValido : IEquatable<VideoValido>
    {
        private StringNaoVaziaOuComEspacos valor = default!;

        public string Valor
        {
            get { return valor; }
            set
            {
                ValidarSeEhArquivoDeVideo(value);
                valor = value;
            }
        }

        public VideoValido(string value)
        {
            Valor = value;
        }

        public static implicit operator VideoValido(string value)
        {
            return new VideoValido(value);
        }

        public static implicit operator string(VideoValido valor)
        {
            return valor.ToString();
        }

        public override string ToString()
            => Valor;
        
        public bool Equals(VideoValido other)
        {
            if (other == null) return false;

            return this.Valor == other.Valor;
        }

        private static void ValidarSeEhArquivoDeVideo(string value)
        {
            string[] extensoesValidas = { ".mp4", ".avi", ".mov", ".mkv", ".flv", ".wmv" };
            if (!extensoesValidas.Any(extensao => value.EndsWith(extensao, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("O valor atribuído não representa um arquivo de vídeo com formato válido");
            }
        } 
    }
}