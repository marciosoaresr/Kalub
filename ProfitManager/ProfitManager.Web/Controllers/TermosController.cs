using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers
{
    public class TermosController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.termo = FuncoesTermoUso.Load(1);
            return View();
        }

    }
}
