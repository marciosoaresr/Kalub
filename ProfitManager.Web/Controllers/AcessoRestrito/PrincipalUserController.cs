using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using iugu.net.Entity;
using iugu.net.Request;
using iugu.net.Lib;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Negocio.Repositorio;
using ProfitManager.Web.RepositorioWeb;


namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    public class PrincipalUserController : BaseController
    {

        public ActionResult Index()
        {

            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                EmpresaUsuario empresaUsuario =
                    FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);


                bool acessoMobile = false;
                if (VerificaMobile())
                {
                    acessoMobile = true;
                }
                ViewBag.Mobile = acessoMobile;

                ViewBag.status = empresaUsuario.Empresa.Status;


                if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
                {
                    return RedirectToAction("Suspenso", "EmpresaRecebimento");
                }

                //**** PERIODO USAO GRATIS 7 DIAS
                string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
                ViewBag.diasGratis = msgPeriodoGratis;
                if (ViewBag.status == EnumEmpresaStatus.Gratis)
                {
                    string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                    int dia = Convert.ToInt32(dias);
                    if (dia <= 0)
                    {
                        return RedirectToAction("Contrate", "EmpresaRecebimento");
                    }
                }
                //****


                //**** PERIODO DE USO ALUNO/PROFESSOR/CONSULTOR 60 DIAS
                //string msgDiasUso = FuncoesEmpresa.MensagemDiasUso(empresaUsuario.Empresa.Id);
                //ViewBag.diasUso = msgDiasUso;
                //if (ViewBag.status == EnumEmpresaStatus.TipoPlano)
                //{
                //    string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                //    int dia = Convert.ToInt32(dias);
                //    if (dia <= 0)
                //    {
                //        return RedirectToAction("PeriodoEncerrado", "EmpresaRecebimento");
                //    }
                //}
                //****

                //ViewBag.ListaDRE = FuncoesGrupoDre.CarregaDre(empresaUsuario.Empresa.Id, DateTime.Today);

                if (empresaUsuario != null)
                {
                    var dataLancamento = DateTime.Now;
                    var listaSaldoInicialDiaAnterior =
                    FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
                    ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

                    ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
                    ViewBag.EmpresaEmail = empresaUsuario.Empresa.Email;
                    ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
                    ViewBag.CategoriaEmpresa = empresaUsuario.Empresa.CategoriaEmpresa.Id;
                    var nomeCategoria = FuncoesCategoriaEmpresa.Load(empresaUsuario.Empresa.CategoriaEmpresa.Id);
                    ViewBag.NomeCategoria = nomeCategoria.CampoNome.Nome;
                }

                DateTime dataMesAnterior = DateTime.Today;
                dataMesAnterior = dataMesAnterior.AddMonths(-1);
                dataMesAnterior = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
                ViewBag.listaSaldoinicialMesAnterior = FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataMesAnterior, true);

                var list = FuncoesExtratoConta.CarregaExtrato(empresaUsuario.Empresa.Id, DateTime.Today, 1);

                List<ExtratoConta> listExtratoContaAumenta = list.Where(x => x.Saldo > 0).ToList();
                List<ExtratoConta> listExtratoContaDiminui = list.Where(x => x.Saldo < 0).ToList();
                ViewBag.totalAumentoSaldo = listExtratoContaAumenta.Sum(x => x.Saldo);
                ViewBag.totalReducaoSaldo = Math.Abs(listExtratoContaDiminui.Sum(x => x.Saldo));
                ViewBag.saldoFinal = FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataMesAnterior, true) + ViewBag.totalAumentoSaldo - ViewBag.totalReducaoSaldo;

                //Mês atual
                List<DemonstracaoFluxoCaixa> listDfc = FuncoesDfc.CarregaDfc(empresaUsuario.Empresa.Id, DateTime.Today);
                ViewBag.entradasOperacionais = listDfc.Where(x => x.Codigo.StartsWith("1.01") && x.TipoDfc == "O" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasOperacionais = listDfc.Where(x => x.Codigo.StartsWith("2.01") && x.TipoDfc == "O" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.entradasInvestimentos = listDfc.Where(x => x.Codigo.StartsWith("1.02") && x.TipoDfc == "I" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasInvestimentos = listDfc.Where(x => x.Codigo.StartsWith("2.02") && x.TipoDfc == "I" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.entradasFinanciamento = listDfc.Where(x => x.Codigo.StartsWith("1.03") && x.TipoDfc == "F" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasFinanciamento = listDfc.Where(x => x.Codigo.StartsWith("2.03") && x.TipoDfc == "F" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                //Mês anterior
                List<DemonstracaoFluxoCaixa> listDfcMesAnterior = FuncoesDfc.CarregaDfc(empresaUsuario.Empresa.Id, dataMesAnterior);
                ViewBag.entradasOperacionaisMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("1.01") && x.TipoDfc == "O" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasOperacionaisMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("2.01") && x.TipoDfc == "O" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.entradasInvestimentosMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("1.02") && x.TipoDfc == "I" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasInvestimentosMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("2.02") && x.TipoDfc == "I" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.entradasFinanciamentoMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("1.03") && x.TipoDfc == "F" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));
                ViewBag.saidasFinanciamentoMesAnterior = listDfcMesAnterior.Where(x => x.Codigo.StartsWith("2.03") && x.TipoDfc == "F" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

                DateTime dataInical = DateTime.Today;
                string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataInical.Month);
                ViewBag.mesExtAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);

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
        }

        public ActionResult Manual()
        {
            EmpresaUsuario empresaUsuario =
                    FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

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

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            ViewBag.manual = FuncoesManual.Load(1);

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
