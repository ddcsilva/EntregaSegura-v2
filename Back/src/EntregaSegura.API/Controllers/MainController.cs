using EntregaSegura.Domain.Entities;
using EntregaSegura.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace EntregaSegura.API.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotificadorErros _notificadorErros;

        protected MainController(INotificadorErros notificadorErros)
        {
            _notificadorErros = notificadorErros;
        }

        protected bool OperacaoValida()
        {
            return !_notificadorErros.TemNotificacoes();
        }

        protected ActionResult CustomResponse(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new
            {
                success = OperacaoValida(),
                statusCode = (int)statusCode,
                data = result,
                errors = OperacaoValida() ? new string[] { } : _notificadorErros.ObterNotificacoes().Select(n => n.Mensagem)
            };

            return StatusCode((int)statusCode, response);
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                NotificarErroModelInvalida(modelState);
            }

            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var mensagemErro = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(mensagemErro);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificadorErros.Handle(new NotificacaoErros(mensagem));
        }
    }
}
