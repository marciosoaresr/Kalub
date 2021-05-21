using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class TipoDocumentoFechamentoCaixaController : Controller
    {

        public ActionResult Index(int? pageNumber, string search)
        {
            List<TipoDocumentoFechamentoCaixa> listTipoDocumento = (!string.IsNullOrEmpty(search)) ? FuncoesTipoDocumentoFechamentoCaixa.Load(search) : FuncoesTipoDocumentoFechamentoCaixa.Load();

            const int pageSize = 10;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listTipoDocumento != null && listTipoDocumento.Count > 0 ? (listTipoDocumento.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            return View(listTipoDocumento.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesTipoDocumentoFechamentoCaixa.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarTipoDocumento(int? id)
        {
            var tipodocumento = (id.HasValue) ? FuncoesTipoDocumentoFechamentoCaixa.Load((int)id) : new TipoDocumentoFechamentoCaixa();

            return PartialView("CadastrarEditarTipoDocumento", tipodocumento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarTipoDocumento(TipoDocumentoFechamentoCaixa tipodocumento, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesTipoDocumentoFechamentoCaixa.Salvar(tipodocumento);
                Thread.Sleep(1000);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", "CadastrarEditarTipoDocumento");
            }


        }
    }
}
