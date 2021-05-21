using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers
{
    public class PrecoController : Controller
    {
        
        public ActionResult Index()
        {

            return View();
        }

    }
}
