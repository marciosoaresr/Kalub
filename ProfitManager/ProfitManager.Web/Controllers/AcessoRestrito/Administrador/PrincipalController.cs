using System.Web.Mvc;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    [HandleError]
    public class PrincipalController : BaseController
    {
        public ActionResult Index()
        {
            string periodo = "hoje";
            ViewBag.totalValorHoje = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalHoje = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);

            periodo = "ontem";
            ViewBag.totalValorOntem = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalOntem = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);

            periodo = "sete-dias";
            ViewBag.totalValorSeteDias = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalSeteDias = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);

            periodo = "mes-atual";
            ViewBag.totalValorMesAtual = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalMesAtual = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);

            periodo = "mes-anterior";
            ViewBag.totalValorMesAnterior = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalMesAnterior = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);

            periodo = "todos";
            ViewBag.totalValorTodos = FuncoesEmpresaRecebimento.CarregaTotalValorAssinaturas(periodo);
            ViewBag.totalTodos = FuncoesEmpresaRecebimento.CarregaTotalAssinaturas(periodo);


            periodo = "hoje";
            ViewBag.totaleHoje = FuncoesEmpresa.CarregaTotalEmpresas(periodo);

            periodo = "ontem";
            ViewBag.totaleOntem = FuncoesEmpresa.CarregaTotalEmpresas(periodo);

            periodo = "sete-dias";
            ViewBag.totaleSeteDias = FuncoesEmpresa.CarregaTotalEmpresas(periodo);

            periodo = "mes-atual";
            ViewBag.totaleMesAtual = FuncoesEmpresa.CarregaTotalEmpresas(periodo);

            periodo = "mes-anterior";
            ViewBag.totaleMesAnterior = FuncoesEmpresa.CarregaTotalEmpresas(periodo);

            periodo = "todos";
            ViewBag.totaleTodos = FuncoesEmpresa.CarregaTotalEmpresas(periodo);
            return View();
        }

    }
}
