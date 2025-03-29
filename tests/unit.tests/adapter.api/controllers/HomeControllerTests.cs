using adapter.api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace unit.tests.adapter.api.controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void GetDeveRetornarOkResult()
        {
            var controller = new HomeController();

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void GetDeveRetornarJsonEsperado()
        {
            var controller = new HomeController();

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            string responseValue = okResult?.Value?.ToString()!;
            Assert.Contains("Live!",responseValue);

        }
    }
}