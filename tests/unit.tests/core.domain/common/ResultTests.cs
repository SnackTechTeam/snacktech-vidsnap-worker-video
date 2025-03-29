using core.domain.common;

namespace unit.tests.core.domain.common
{
    public class ResultTests
    {
        [Fact]
        public void ConstrutorBaseDeveTerSucessoTrueMensagemVaziaEExceptionNull()
        {
            var result = new Result();

            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void ConstrutorComMensagemDeveTerSucessoFalseEExceptionNull()
        {
            var message = "Error occurred";
            var result = new Result(message);

            Assert.False(result.Success);
            Assert.Equal(message, result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void ConstrutorComExceptionDeveTerSucessoFalseEMensagemVazia()
        {
            var exception = new Exception("Test exception");
            var result = new Result(exception);

            Assert.False(result.Success);
            Assert.Equal(exception.Message, result.Message);
            Assert.Equal(exception, result.Exception);
        }

        [Fact]
        public void IsSuccessDeveRetornarValorCorreto()
        {
            var successResult = new Result();
            var failureResult = new Result("Error");

            Assert.True(successResult.IsSuccess());
            Assert.False(failureResult.IsSuccess());
        }

        [Fact]
        public void HasExceptionDeveRetornarValorCorreto()
        {
            var resultWithException = new Result(new Exception("Test exception"));
            var resultWithoutException = new Result();

            Assert.True(resultWithException.HasException());
            Assert.False(resultWithoutException.HasException());
        }
    }

    public class ResultGenericTest
    {
        [Fact]
        public void ConstrutorComDadosDeveTerSucessoTrueEValorPreenchido()
        {
            var data = 42;
            var result = new Result<int>(data);

            Assert.True(result.Success);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void ConstrutorComMensagemEIsErrorTrueDeveTerSucessoFalse()
        {
            var message = "Error occurred";
            var result = new Result<string>(message, true);

            Assert.False(result.Success);
            Assert.Equal(message, result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public void ConstrutorComMensagemEErrorFalseDeveLancarException()
        {
            var message = "This should fail";

            Assert.Throws<ArgumentException>(() => new Result<string>(message, false));
        }

        [Fact]
        public void ConstrutorComExceptionDeveTerSucessoFalseMensagemPreenchidaEExceptionPreenchida()
        {
            var exception = new Exception("Test exception");
            var result = new Result<int>(exception);

            Assert.False(result.Success);
            Assert.Equal(exception.Message, result.Message);
            Assert.Equal(exception, result.Exception);
            Assert.Equal(default(int), result.Data);
        }

        [Fact]
        public void GetValueDeveRetornarValorCorreto()
        {
            var data = "Test data";
            var result = new Result<string>(data);

            Assert.Equal(data, result.GetValue());
        }
    }
}