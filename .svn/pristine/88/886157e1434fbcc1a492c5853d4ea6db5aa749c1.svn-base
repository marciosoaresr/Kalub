using System.Web.Mvc;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class ParametrosController : Controller
    {
        public ActionResult Index()
        {
            var parametros = FuncoesParametros.Load(1);
            return View(parametros);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarParametros(Parametros parametros, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesParametros.Salvar(parametros);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", "Index");
            }
        }




    }
}
