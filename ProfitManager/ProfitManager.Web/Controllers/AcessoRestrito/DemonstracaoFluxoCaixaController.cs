﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.WebPages;
using Kendo.Mvc.Extensions;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;
using ProfitManager.Web.ViewModels;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    public class DemonstracaoFluxoCaixaController : Controller
    {

        public ActionResult Index()
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
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
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

        [HttpPost]
        public ActionResult SetDataDfc(DemonstracaoFluxoCaixa dfc)
        {
            return RedirectToAction("VisualizarDfc", new { dataAtual = dfc.DataAtual, dataInicial = dfc.DataInicial, tipoDfc = dfc.tipo_dfc });
        }

       
        public ActionResult VisualizarDfc(DateTime dataAtual, DateTime dataInicial, string tipoDfc)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            if (dataInicial == Convert.ToDateTime("01/01/0001"))
            {
                dataInicial = dataAtual;
            }

            List<DemonstracaoFluxoCaixa> listDfc = FuncoesDfc.CarregaDfc(empresaUsuario.Empresa.Id, dataInicial);

            ViewBag.entradasOperacionais = listDfc.Where(x => x.Codigo.StartsWith("1.01") && x.TipoDfc == "O" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.saidasOperacionais = listDfc.Where(x => x.Codigo.StartsWith("2.01") && x.TipoDfc == "O" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.entradasInvestimentos = listDfc.Where(x => x.Codigo.StartsWith("1.02") && x.TipoDfc == "I" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.saidasInvestimentos = listDfc.Where(x => x.Codigo.StartsWith("2.02") && x.TipoDfc == "I" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.entradasFinanciamento = listDfc.Where(x => x.Codigo.StartsWith("1.03") && x.TipoDfc == "F" && x.Area == 5 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.saidasFinanciamento = listDfc.Where(x => x.Codigo.StartsWith("2.03") && x.TipoDfc == "F" && x.Area == 6 && (x.Saldo != 0 || x.SaldoAcumulado != 0));

            ViewBag.saldoinicialMesAnterior =
                FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1,
                    new DateTime(dataInicial.Year, dataInicial.Month, 1).AddDays(-1), true);

            ViewBag.saldoinicialAnoAnterior =
                FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1,
                    new DateTime(dataInicial.Year, 1, 1).AddDays(-1), true);

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.Logomarca = empresaUsuario.Empresa.Logomarca;

            ViewBag.TipoDfc = tipoDfc;
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
