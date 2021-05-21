using System.Web.Mvc;
using ProfitManager.Web.Security;

namespace ProfitManager.Web.Controllers
{
    [AutorizacaoDeAcesso]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

    }
}
