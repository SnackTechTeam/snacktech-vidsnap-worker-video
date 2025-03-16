using core.domain.types;

namespace core.domain.models
{
    public record VideoParaBaixar
    {
        public required StringNaoVaziaOuComEspacos CaminhoLocal;
        public required StringNaoVaziaOuComEspacos DestinoLocal;
        public NumeroNaoNegativoOuZero IntervaloEmSegundos;
    }
}