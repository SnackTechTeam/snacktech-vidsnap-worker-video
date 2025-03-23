using core.domain.common;

namespace unit.tests.core.domain.common
{
    public class ConstantsValuesTest
    {
        [Fact]
        public void IntervaloPadraoEmSegundosDeveSer20()
        {
            int actualValue = ConstantsValues.IntervaloPadraoEmSegundos;

            Assert.Equal(20, actualValue);
        }
    }
}