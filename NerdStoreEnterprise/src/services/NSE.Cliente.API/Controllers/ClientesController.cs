using Microsoft.AspNetCore.Mvc;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Models;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Usuario;

namespace NSE.Cliente.API.Controllers
{
    public class ClientesController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAspNetUser _user;
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IClienteRepository clienteRepository, IAspNetUser user, IMediatorHandler mediatorHandler)
        {
            _clienteRepository = clienteRepository;
            _user = user;
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("cliente/endereco")]
        public async Task<ActionResult> ObterEndereco()
        {
            var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());
            return endereco is null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediatorHandler.EnviarComando(endereco));
        }


    }
}
