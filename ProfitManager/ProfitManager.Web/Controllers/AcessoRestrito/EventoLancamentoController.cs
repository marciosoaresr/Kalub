using System;
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
    [HandleError]
    public class EventoLancamentoController : BaseController
    {

        public ActionResult FechamentoCaixa()
        {
            EmpresaUsuario empresaUsuario =
                    FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            var empresa = FuncoesEmpresaUsuario.BuscarEmpresa(empresaUsuario.Empresa.Id);
            ViewBag.status = empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            //**** PERIODO USAO GRATIS 30 DIAS
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
            string msgDiasUso = FuncoesEmpresa.MensagemDiasUso(empresaUsuario.Empresa.Id);
            ViewBag.diasUso = msgDiasUso;
            if (ViewBag.status == EnumEmpresaStatus.TipoPlano)
            {
                string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                int dia = Convert.ToInt32(dias);
                if (dia <= 0)
                {
                    return RedirectToAction("PeriodoEncerrado", "EmpresaRecebimento");
                }
            }
            //****

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
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
        public ActionResult Index(string novaData)
        {
            EmpresaUsuario empresaUsuario =
                    FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            var eventoLancamento = new EventoLancamento();
            eventoLancamento.Data = DateTime.Now;
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;


            var empresa = FuncoesEmpresaUsuario.BuscarEmpresa(empresaUsuario.Empresa.Id);
            ViewBag.status = empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            //**** PERIODO USAO GRATIS 30 DIAS
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
            string msgDiasUso = FuncoesEmpresa.MensagemDiasUso(empresaUsuario.Empresa.Id);
            ViewBag.diasUso = msgDiasUso;
            if (ViewBag.status == EnumEmpresaStatus.TipoPlano)
            {
                string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
                int dia = Convert.ToInt32(dias);
                if (dia <= 0)
                {
                    return RedirectToAction("PeriodoEncerrado", "EmpresaRecebimento");
                }
            }
            //****

            if (!novaData.HasValue())
            {
                DateTime dataSession = RepositorioWebUsuario.GetDateQueryCookie();
                if (dataSession != DateTime.MinValue)
                {
                    return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = dataSession, idExercicio = 0 });
                }
            }

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
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

            return View(eventoLancamento);
        }

        [HttpGet]
        public ActionResult AbreEventoHelp(int? id)
        {
            var evento = FuncoesEventoLancamento.CarregaEventoHelp((int)id);
            var nomeEvento = FuncoesEventoLancamento.LoadTipoEvento((int)id);
            ViewBag.eventoHelp = evento.Evento.Help;
            ViewBag.NomeEvento = nomeEvento.CampoNome.Nome;
            ViewBag.contas = FuncoesEventoOperacao.LoadContasEventoOperacao((int)id);
            return PartialView("AbreEventoHelp", evento);
        }

        [HttpGet]
        public ActionResult AbreExercicio(int? id)
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ?? new EmpresaUsuario();

            List<ExercicioEmpresa> listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Id).OrderBy(x => x.Id).ToList();

            var exercicio = new Exercicio();

            foreach (var exercicioempresa in listExercicioEmpresa)
            {
                var exercicioItem = FuncoesExercicioItem.LoadExercicioItem(exercicioempresa.Exercicio.Id);
                foreach (var exerciciosItem in exercicioItem)
                {
                    exercicio.AddListExercicioItem(exerciciosItem);
                }
            }
            exercicio.Empresa = empresaUsuario.Empresa;

            return PartialView("AbreExercicio", exercicio);
        }

        [HttpGet]
        public ActionResult CadastrarEditarNotaExplicativa(int? id)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            DateTime dataSession = RepositorioWebUsuario.GetDateQueryCookie();
            var tipoEvento = FuncoesEventoLancamento.LoadTipoEvento((int)id);
            if (tipoEvento.TipoEventoLancamento == EnumTipoEventoLancamento.Diario)
            {
                var evento = FuncoesEventoLancamento.LoadNotaExplicativa((int)id, empresaUsuario.Empresa.Id,
                    dataSession);
                return PartialView("CadastrarEditarNotaExplicativa", evento);
            }
            else
            {
                DateTime dataPrimeiroDiaMes = new DateTime(dataSession.Year, dataSession.Month, 1);
                var evento = FuncoesEventoLancamento.LoadNotaExplicativa((int)id, empresaUsuario.Empresa.Id,
                    dataPrimeiroDiaMes);
                return PartialView("CadastrarEditarNotaExplicativa", evento);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarNotaExplicativa(EventoLancamento eventoLancamento)
        {
            DateTime dataSession = RepositorioWebUsuario.GetDateQueryCookie();
            var dataPrimeiroDiaMes = new DateTime(dataSession.Year, dataSession.Month, 1);

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            var evento = FuncoesEventoLancamento.LoadNotaExplicativa(eventoLancamento.Id, empresaUsuario.Empresa.Id, dataSession);
            if (evento == null)
            {
                evento = FuncoesEventoLancamento.LoadNotaExplicativa(eventoLancamento.Id, empresaUsuario.Empresa.Id, dataPrimeiroDiaMes);
            }

            if (evento != null)
            {
                evento.NotaExplicativa = eventoLancamento.NotaExplicativa;

                ModelState["Evento"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    FuncoesEventoLancamento.Salvar(evento);
                    return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = dataSession, idExercicio = 0 });//eventoLancamento.Data});
                }
            }
            else
            {
                return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = dataSession, idExercicio = 0 });

            }


            return View();
        }

        [HttpPost]
        public ActionResult SetDataEventoLancamento(EventoLancamento eventoLancamento)
        {

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            var listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();
            bool acessoExercicio = listExercicioEmpresa.Count > 0;
            ViewBag.existeExercicio = false;
            ViewBag.tipoCadastroProfessor = tipo;
            if (tipo == EnumTipoCadastro.Professor && acessoExercicio)
            {
                Exercicio exercicio = FuncoesExercicio.CarregaExercicio(empresaUsuario.Empresa.Id);
                if (eventoLancamento.Data < exercicio.DataInicio || eventoLancamento.Data > exercicio.DataFim)
                {
                    TempData["mensagem"] =
                        "Data de lançamento não pode ser diferente que a data Inicial e Final do seu exercicio";
                    return RedirectToAction("Index", new {data = ""});
                }
                RepositorioWebUsuario.SetDateQueryCookie(eventoLancamento.Data);
            }
            RepositorioWebUsuario.SetDateQueryCookie(eventoLancamento.Data);

            return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = eventoLancamento.Data, idExercicio = 0 });
        }

        [HttpPost]
        public ActionResult SetDataFechamentoCaixa(EventoLancamento eventoLancamento)
        {
            RepositorioWebUsuario.SetDateQueryCookie(eventoLancamento.Data);
            return RedirectToAction("ImprimirLancamentos", new { dataLancamentos = eventoLancamento.Data });
        }

        public ActionResult VisualizarEventoLancamento(DateTime dataLancamentos, int idExercicio)
        {

            if (idExercicio != 0)
            {
                var listExercicio = FuncoesExercicio.Load(idExercicio);
                Empresa empresa = FuncoesEmpresa.Load(listExercicio.Empresa.Id);
                empresa.CategoriaEmpresa = listExercicio.CategoriaEmpresa;
                FuncoesEmpresa.Salvar(empresa);
            }

            EmpresaUsuario empresaUsuario =
                   FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.CategoriaEmpresa = empresaUsuario.Empresa.CategoriaEmpresa.Id;

            ViewBag.DataLancamento = dataLancamentos;
            List<EventoLancamentoViewModel> listLancamentosPeriodo = LancamentosPeriodo(dataLancamentos);

            List<FechamentoCaixa> listTipoDocumento = LancamentosFechamentoCaixa(dataLancamentos);
            ViewBag.listaTipoDocumento = listTipoDocumento;

            string dias = FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasVencimento = dias;
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

            //var dataLancamento = DateTime.Now;
            //var listaSaldoInicialDiaAnterior =
            //FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            //ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

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

            return View(listLancamentosPeriodo);
        }

        private List<FechamentoCaixa> LancamentosFechamentoCaixa(DateTime dataLancamento)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            int empresa = empresaUsuario.Empresa.Id;

            List<TipoDocumentoFechamentoCaixa> listTipoDocumento = FuncoesTipoDocumentoFechamentoCaixa.Load();

            var listFechamentoCaixa = FuncoesFechamentoCaixa.LoadByDocumentoAndEmpresaAndData(listTipoDocumento, empresa, dataLancamento);

            var valorNovoDia = new List<FechamentoCaixa>();
            valorNovoDia = FuncoesFechamentoCaixa.CarregaValorDiario(listTipoDocumento.Select(x => x.Id).ToList(), empresaUsuario.Empresa.Id, dataLancamento);

            foreach (var tipodocumento in listTipoDocumento)
            {
                var tipodocumentoExistente = valorNovoDia.FirstOrDefault(x => x.TipoDocumentoFechamentoCaixa.Id == tipodocumento.Id);
                if (listFechamentoCaixa.All(x => x.TipoDocumentoFechamentoCaixa.Id != tipodocumento.Id))
                    listFechamentoCaixa.Add(new FechamentoCaixa()
                    {
                        Data = dataLancamento,
                        TipoDocumentoFechamentoCaixa = tipodocumento,
                        Empresa = empresaUsuario.Empresa,
                        Valor = tipodocumentoExistente != null ? tipodocumentoExistente.Valor : 0,
                    });
            }

            var totalDiaFechamentoCaixa = listFechamentoCaixa.Sum(x => x.Valor);
            ViewBag.TotalDiaFechamentoCaixa = totalDiaFechamentoCaixa;

            return listFechamentoCaixa;

        }



        private List<EventoLancamentoViewModel> LancamentosPeriodo(DateTime dataLancamento)
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            List<Evento> listEvento = FuncoesEvento.LoadEventosPorTipo(empresaUsuario.Empresa.CategoriaEmpresa.Id);

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            List<ExercicioEmpresa> listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            bool acessoExercicio = listExercicioEmpresa.Count > 0;
            if (tipo == EnumTipoCadastro.Professor && acessoExercicio)
            {
                listEvento = FuncoesEvento.LoadEventosPorExercicio(empresaUsuario.Empresa.CategoriaEmpresa.Id);
            }

            var listEventoLancamento = FuncoesEventoLancamento.LoadByGrupoAreaAndEmpresaAndData(listEvento, empresaUsuario.Empresa.Id, dataLancamento);

            var valoresMensalNovoDia = new List<EventoLancamento>();

            valoresMensalNovoDia = FuncoesEventoLancamento.CarregaValorMensal(listEvento.Select(x => x.Id).ToList(), empresaUsuario.Empresa.Id, dataLancamento);

            foreach (var evento in listEvento)
            {
                var eventoExistente = valoresMensalNovoDia.FirstOrDefault(x => x.Evento.Id == evento.Id);
                if (listEventoLancamento.All(x => x.Evento.Id != evento.Id))
                    listEventoLancamento.Add(new EventoLancamento
                    {
                        Data = dataLancamento,
                        Evento = evento,
                        Empresa = empresaUsuario.Empresa,
                        ValorMes = eventoExistente != null ? eventoExistente.ValorMes : 0
                    });
            }

            var listEventoLancamentoViewModel = new List<EventoLancamentoViewModel>();

            foreach (var eventoLancamentoGroup in listEventoLancamento)
            {
                var eventoLancamentoViewModel = new EventoLancamentoViewModel
                {
                    IdEvento = eventoLancamentoGroup.Evento.Id,
                    Data = eventoLancamentoGroup.Data,
                    IdEmpresa = eventoLancamentoGroup.Empresa.Id,
                    Evento = eventoLancamentoGroup.Evento.CampoNome.Nome,
                    EventoHelper = eventoLancamentoGroup.Evento.CampoHelp.Nome,
                    Area = eventoLancamentoGroup.Evento.Area,
                    SubArea = eventoLancamentoGroup.Evento.SubArea,
                    ValorDia = eventoLancamentoGroup.Valor,
                    ValorMes = eventoLancamentoGroup.ValorMes,
                    NotaExplicativa = eventoLancamentoGroup.NotaExplicativa,
                    TipoEventoLancamento = eventoLancamentoGroup.Evento.TipoEventoLancamento,
                    MaisUsado = eventoLancamentoGroup.Evento.MaisUsado,
                    Help = eventoLancamentoGroup.Evento.Help,
                    Ordem = eventoLancamentoGroup.Evento.Ordem
                };

                listEventoLancamentoViewModel.Add(eventoLancamentoViewModel);
            }

            var listaSaldoInicialDiaAnterior =
                FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1,
                    dataLancamento.AddDays(-1), true);

            DateTime dataMesAnterior = dataLancamento;
            dataMesAnterior = dataMesAnterior.AddMonths(-1);
            dataMesAnterior = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
            var listaSaldoinicialMesAnterior = FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataMesAnterior, true);

            var totalDia = listEventoLancamento.Where(x => x.Evento.TipoEventoLancamento == EnumTipoEventoLancamento.Diario && x.Evento.Area.Codigo == "0001").Sum(x => x.Valor);

            var totalMes = listEventoLancamento.Where(x => x.Evento.Area.Codigo == "0001").Sum(x => x.ValorMes);

            var totalDiaSaida = listEventoLancamento.Where(x => x.Evento.TipoEventoLancamento == EnumTipoEventoLancamento.Diario && x.Evento.Area.Codigo == "0002").Sum(x => x.Valor);

            var totalMesSaida = listEventoLancamento.Where(x => x.Evento.Area.Codigo == "0002").Sum(x => x.ValorMes);

            var totalDiaMovimento = listEventoLancamento.Where(x => x.Evento.TipoEventoLancamento == EnumTipoEventoLancamento.Diario && x.Evento.Area.Codigo == "0003").Sum(x => x.Valor);

            var totalMesMovimento = listEventoLancamento.Where(x => x.Evento.Area.Codigo == "0003").Sum(x => x.ValorMes);

            ViewBag.valorTotalEntradasDiaAnterior =
                        FuncoesEventoLancamento.CarregaValorTotalMesAtual(empresaUsuario.Empresa.Id, dataLancamento.AddDays(-1),
                            EnumTipoEventoLancamento.Diario, 5);
            ViewBag.valorTotalSaidasDiaAnterior =
                FuncoesEventoLancamento.CarregaValorTotalMesAtual(empresaUsuario.Empresa.Id, dataLancamento.AddDays(-1),
                    EnumTipoEventoLancamento.Diario, 6);

            ViewBag.TotalDia = totalDia;
            ViewBag.TotalMes = totalMes;

            ViewBag.TotalDiaSaida = totalDiaSaida;
            ViewBag.TotalMesSaida = totalMesSaida;

            ViewBag.TotalDiaMovimento = totalDiaMovimento;
            ViewBag.TotalMesMovimento = totalMesMovimento;

            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;
            ViewBag.TotalSaldoinicialMesAnteriorContaCaixa = listaSaldoinicialMesAnterior;

            return listEventoLancamentoViewModel;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update(FormCollection form)
        {
            var dataLancamento = form["dataLancamento"].AsDateTime();
            var eventosdiarios = form.AllKeys.Where(x => x.StartsWith("D"));
            var eventosmensal = form.AllKeys.Where(x => x.StartsWith("M"));
            var fechamentoscaixadiarios = form.AllKeys.Where(x => x.StartsWith("FD"));

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(false).Id);

            var listEventos = new List<EventoLancamento>();
            foreach (var item in eventosdiarios)
            {
                int idEvento = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));

                var eventoLancamento = new EventoLancamento
                {
                    Data = dataLancamento.Date,
                    Evento = new Evento
                    {
                        Id = idEvento
                    },
                    Valor = Convert.ToDecimal(form[item].Replace(".", "")),
                    Empresa = empresaUsuario.Empresa
                };

                listEventos.Add(eventoLancamento);
            }

            foreach (var item in eventosmensal)
            {
                int idEvento = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));

                var eventoLancamento = new EventoLancamento
                {
                    Data = new DateTime(dataLancamento.Year, dataLancamento.Month, 1),
                    Evento = new Evento
                    {
                        Id = idEvento
                    },
                    Valor = Convert.ToDecimal(form[item].Replace(".", "")),
                    Empresa = empresaUsuario.Empresa
                };

                listEventos.Add(eventoLancamento);
            }

            FuncoesEventoLancamento.SalvarOuAtualizarLancamentos(listEventos, empresaUsuario.Empresa.Id);


            var listFechamentosDiario = new List<FechamentoCaixa>();
            foreach (var item in fechamentoscaixadiarios)
            {
                int idTipoDocumento = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));

                var fechamentoCaixa = new FechamentoCaixa
                {
                    Data = dataLancamento.Date,
                    TipoDocumentoFechamentoCaixa = new TipoDocumentoFechamentoCaixa
                    {
                        Id = idTipoDocumento
                    },
                    Valor = Convert.ToDecimal(form[item]),
                    Empresa = empresaUsuario.Empresa,
                };

                listFechamentosDiario.Add(fechamentoCaixa);
            }
            FuncoesFechamentoCaixa.SalvarOuAtualizarFechamentos(listFechamentosDiario, dataLancamento, empresaUsuario.Empresa.Id);

            TempData["mensagem"] = "Lançamentos gravados com sucesso!";

            return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = dataLancamento, idExercicio = 0 });
        }

        public JsonResult GetEventos(string text)
        {
            var listEvento = new List<Evento>();

            if (!string.IsNullOrEmpty(text.Trim()))
            {
                listEvento = FuncoesEvento.Load(text);
            }

            var listEventReturn =
                listEvento.Select(
                    evento => new Evento { Id = evento.Id, Codigo = evento.Codigo, CampoNome = evento.CampoNome })
                    .ToList();

            return Json(listEventReturn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImprimirLancamentos(DateTime dataLancamentos)
        {

            EmpresaUsuario empresaUsuario =
                   FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.DataLancamento = dataLancamentos;
            List<EventoLancamentoViewModel> listLancamentosPeriodo = LancamentosPeriodo(dataLancamentos);

            List<FechamentoCaixa> listTipoDocumento = LancamentosFechamentoCaixa(dataLancamentos);
            ViewBag.listaTipoDocumento = listTipoDocumento;
            ViewBag.dataFechamento = dataLancamentos.Date;
            ViewBag.Logomarca = empresaUsuario.Empresa.Logomarca;

            return View(listLancamentosPeriodo);
        }

        public ActionResult LancamentoExercicio(int id)
        {
            var listExercicios = FuncoesExercicio.Load(id);
            return RedirectToAction("VisualizarEventoLancamento", new { dataLancamentos = listExercicios.DataInicio.ToString("MM/dd/yyyy"), idExercicio = id });
        }

    }
}
