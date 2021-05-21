using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Security
{
    [HandleError]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AutorizacaoDeAcesso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            if (controller != "Login" || action != "Login")
            {
                Usuario usuario = RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(false);

                if (usuario == null)
                {
                    filterContext.RequestContext.HttpContext.Response.Redirect("/Login/Login?Url=" +
                                                                               filterContext.HttpContext.Request.Url
                                                                                   .LocalPath);
                }
                else
                {
                    if (usuario.Administrador == EnumSimNao.Nao && PrivateControllerAdmin().Contains(controller))
                    {
                        filterContext.RequestContext.HttpContext.Response.Redirect("/PrincipalUser");
                    }
                    else if (usuario.Administrador == EnumSimNao.Sim && PrivateControllerUser().Contains(controller))
                    {
                        filterContext.RequestContext.HttpContext.Response.Redirect("/Principal");
                    }
                }
            }
        }

        private IEnumerable<string> PrivateControllerAdmin()
        {
            return new string[]{ "Eventos", "PlanoContas", "Principal", "Relatorio", "RelatorioItem" };
        }

        private IEnumerable<string> PrivateControllerUser()
        {
            return new string[] { "Empresa", "EventoLancamento", "PrincipalUser", "RelatorioReadOnly" };
        }

    }
}