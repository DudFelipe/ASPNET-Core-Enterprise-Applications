using Microsoft.AspNetCore.Mvc;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;

namespace NSE.Cliente.API.Controllers
{
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<ActionResult> Index()
        {
            var resultado = await _mediatorHandler.EnviarComando(
                new RegistrarClienteCommand(Guid.NewGuid(), "Eduardo", "edu@edu.com", "26887009013"));

            return CustomResponse(resultado);
        }
    }
}
