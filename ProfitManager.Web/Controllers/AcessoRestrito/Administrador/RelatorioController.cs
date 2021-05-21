using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class RelatorioController : BaseController
    {
        public ActionResult Index(int? pageNumber, string search)
        {
            List<Relatorio> listRelatorio = (search.HasValue())
                ? FuncoesRelatorio.Load(search)
                : FuncoesRelatorio.Load();

            const int pageSize = 10;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listRelatorio != null && listRelatorio.Count > 0 ? (listRelatorio.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;

            return View(listRelatorio.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult CadastrarEditarRelatorio(int? idRelatorio)
        {
            var relatorio = (idRelatorio.HasValue) ? FuncoesRelatorio.Load((int) idRelatorio): new Relatorio();

            return PartialView("CadastrarEditarRelatorio", relatorio);
        }

        [HttpPost]
        public ActionResult CadastrarEditarRelatorio(Relatorio relatorio)
        {
            if (ModelState.IsValid)
            {
                FuncoesRelatorio.Salvar(relatorio);

                return RedirectToAction("Index");
            }

            return View(relatorio);
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesRelatorio.Excluir((int)id);

            return RedirectToAction("Index");
        }

    }
}
    