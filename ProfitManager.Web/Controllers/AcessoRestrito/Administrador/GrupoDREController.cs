using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;


namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class GrupoDreController : Controller
    {

        public ActionResult Index(int? pageNumber, string search)
        {
            List<GrupoDRE> listGrupoDre = (search.HasValue()) ? FuncoesGrupoDre.Load(search) : FuncoesGrupoDre.Load();

            const int pageSize = 100;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listGrupoDre != null && listGrupoDre.Count > 0 ? (listGrupoDre.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            return View(listGrupoDre.ToPagedList(pageCurrent, pageSize));
        }

        public ActionResult GerarDre()
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();

            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasVencimento = Convert.ToInt32(dias);
            ViewBag.status = empresaUsuario.Empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            //**** PERIODO USAO GRATIS 30 DIAS
            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;
            if (ViewBag.status == EnumEmpresaStatus.Gratis)
            {
                //string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                int dia = Convert.ToInt32(dias);
                if (dia <= 0)
                {
                    return RedirectToAction("Contrate", "EmpresaRecebimento");
                }
            }
            //****


            //**** PERIODO DE USO ALUNO/PROFESSOR/CONSULTOR 60 DIAS
            string msgDiasUso = FuncoesEmpresa.MensagemDiasUso(empresaUsuario.Empresa.Id);
            ViewBag.diasUso = msgDiasUso;
            if (ViewBag.status == EnumEmpresaStatus.TipoPlano)
            {
                //string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                int dia = Convert.ToInt32(dias);
                if (dia <= 0)
                {
                    return RedirectToAction("PeriodoEncerrado", "EmpresaRecebimento");
                }
            }
            //****

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1,dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            List<ExercicioEmpresa> listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();
            bool acessoExercicio = listExercicioEmpresa.Count > 0;
            ViewBag.existeExercicio = false;
            ViewBag.tipoCadastroProfessor = tipo;

            if (tipo == EnumTipoCadastro.Professor && acessoExercicio)
            {
                ViewBag.existeExercicio = true;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesGrupoDre.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarGrupoDre(int? id)
        {
            var grupodre = (id.HasValue) ? FuncoesGrupoDre.Load((int)id) : new GrupoDRE();

            return PartialView("CadastrarEditarGrupoDRE", grupodre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarGrupoDre(GrupoDRE grupodre, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesGrupoDre.Salvar(grupodre);
                Thread.Sleep(1000);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index","CadastrarEditarGrupoDRE");
            }
        }

        [HttpPost]
        public ActionResult SetDataGerarDre(GrupoDRE dre)
        {
            return RedirectToAction("VisualizarDre", new { dataAtual = dre.DataAtual, dataInicial = dre.DataInicial, tipo = dre.tipoDre });
        }


        public ActionResult VisualizarDre(DateTime dataAtual, DateTime dataInicial, string tipo)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            if (dataInicial == Convert.ToDateTime("01/01/0001"))
            {
                dataInicial = dataAtual;
            }
            int empresa = empresaUsuario.Empresa.Id;

            ViewBag.ListaDRE = FuncoesGrupoDre.CarregaDre(empresa, dataInicial);

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.Logomarca = empresaUsuario.Empresa.Logomarca;
            ViewBag.TipoDre = tipo;
            ViewBag.mesAtual = dataInicial.Month + "/" + dataInicial.Year;
            return View();
        }

        public bool VerificaMobile()
        {
            if (this.Request.Browser.IsMobileDevice || Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null ||
              (Request.ServerVariables["HTTP_ACCEPT"] != null && Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")))
            {
                return true;
            }
            else if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string[] mobiles = new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi","android",
                    "moto", "iphone", "ipad", "macintosh", "windows phone","linux"
                };

                foreach (string s in mobiles)
                {
                    if (Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }
}
