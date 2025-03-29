using core.domain.dtos;

namespace unit.tests.core.domain.dtos
{
    public class ExceptionResponseTests
    {
        [Fact]
        public void ConstrutorConfiguraTypeCorretamente()
        {
            var exception = new InvalidOperationException();

            var response = new ExceptionResponse(exception);

            Assert.Equal(typeof(InvalidOperationException).FullName, response.Type);
        }

        [Fact]
        public void ConstrutorConfiguraStackCorretamente()
        {
            Exception exception = null!;
            try
            {
                throw new InvalidOperationException("Test exception");
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }

            var response = new ExceptionResponse(exception);

            Assert.Equal(exception.StackTrace, response.Stack);
        }

        [Fact]
        public void ConstrutorConfiguraTargetSiteCorretamente()
        {
            Exception exception = null!;
            try
            {
                throw new InvalidOperationException("Test exception");
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }

            var response = new ExceptionResponse(exception);

            Assert.Equal(exception.TargetSite?.ToString(), response.TargetSite);
        }

        [Fact]
        public void ConstrutorLidaComTargetSiteNulo()
        {
            var exception = new Exception("Test exception");

            var response = new ExceptionResponse(exception);

            Assert.Null(response.TargetSite);
        }
    }
}