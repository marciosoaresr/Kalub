using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class CategoriaEmpresaController : Controller
    {

        public ActionResult Index(int? pageNumber, string search)
        {
            List<CategoriaEmpresa> listCategoriaEmpresa = (search.HasValue()) ? FuncoesCategoriaEmpresa.Load(search) : FuncoesCategoriaEmpresa.Load();

            const int pageSize = 100;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listCategoriaEmpresa != null && listCategoriaEmpresa.Count > 0 ? (listCategoriaEmpresa.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            return View(listCategoriaEmpresa.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesCategoriaEmpresa.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarCategoriaEmpresa(int? id)
        {
            var categoriaempresa = (id.HasValue) ? FuncoesCategoriaEmpresa.Load((int)id) : new CategoriaEmpresa();

            return PartialView("CadastrarEditarCategoriaEmpresa", categoriaempresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarCategoriaEmpresa(CategoriaEmpresa categoriaempresa, CategoriaEmpresaSecundaria categoriaempresasecundaria, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesCategoriaEmpresa.Salvar(categoriaempresa);
                //FuncoesCategoriaEmpresaSecundaria.Salvar(categoriaempresasecundaria);
                Thread.Sleep(1000);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index","CadastrarEditarCategoriaEmpresa");
            }

           // ViewBag.ListaArea = ListArea(evento);
            //ViewBag.ListaContaContabil = ListContaContabil();

            //return View(categoriaempresa);
        }
    }
}
