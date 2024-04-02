using Microsoft.AspNetCore.Mvc;
using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Mensagens.Any())
            {
                //Adicionando mensagens de erro na model state para poder repassar para a view
                foreach(var mensagem in resposta.Errors.Mensagens)
                    ModelState.AddModelError(string.Empty, mensagem);

                return true;
            }

            return false;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
