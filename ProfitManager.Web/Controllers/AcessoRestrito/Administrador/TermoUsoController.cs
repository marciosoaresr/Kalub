using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NHibernate.Mapping;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class TermoUsoController : Controller
    {
        public ActionResult Index()
        {
            var termo = FuncoesTermoUso.Load(1);
            return View(termo);
        }

        //[HttpGet]
        //public ActionResult EditarTermoUso(int? id)
        //{
        //    var diagnostico = (id.HasValue) ? FuncoesDiagnostico.Load((int)id) : new Diagnostico();

        //    return PartialView("CadastrarEditarDiagnostico", diagnostico);
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarTermoUso(TermoUso termouso, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesTermoUso.Salvar(termouso);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", "Index");
            }
        }




    }
}
