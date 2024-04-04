using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
        {
            var response = await _clienteService.AdicionarEndereco(endereco);

            if(ResponsePossuiErros(response))
            {
                TempData["Erros"] = ModelState.Values.SelectMany(e => e.Errors.Select(m => m.ErrorMessage)).ToList();
            }

            return RedirectToAction("EnderecoEntrega", "Pedido");
        }
    }
}
