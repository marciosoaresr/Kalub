using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.EntidadeAuxiliar;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    [HandleError]
    public class RelatorioReadOnlyController : BaseController
    {

        public ActionResult Index(string novaData)
        {
            var evnt = new EventoLancamento();
            evnt.Data = DateTime.Now;

            if (!novaData.HasValue())
            {
                DateTime dataSession = RepositorioWebUsuario.GetDateQueryCookie();
                if (dataSession != DateTime.MinValue)
                {
                    return RedirectToAction("SelecionarRelatorio", new { dataRelatorio = dataSession });
                }
            }

            return View(evnt);
        }

        [HttpGet]
        public ActionResult VisualizarRelatorio(int? idRelatorio, DateTime dataRelatorio)
        {
            if (idRelatorio != null) ViewBag.NomeRelatorio = FuncoesRelatorio.Load((int) idRelatorio).NomeNormalizado;

            ViewBag.DataResultado = dataRelatorio;

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            List<RelatorioItemAuxiliar> listRelItemAux = FuncoesRelatorioItemAuxiliar.BuildReport(
                (int)idRelatorio, dataRelatorio, dataRelatorio, empresaUsuario.Empresa.Id);
            

            return View(listRelItemAux);
        }

        public ActionResult SelecionarRelatorio(DateTime dataRelatorio)
        {
            List<Relatorio> listRelatorio = FuncoesRelatorio.Load();
            ViewBag.DataResultado = dataRelatorio;

            return View(listRelatorio);
        }

        [HttpPost]
        public ActionResult SetDataRelatorio(EventoLancamento eventoLancamento)
        {
            RepositorioWebUsuario.SetDateQueryCookie(eventoLancamento.Data);

            return RedirectToAction("SelecionarRelatorio", new { dataRelatorio = eventoLancamento.Data });
        }


    }
}
