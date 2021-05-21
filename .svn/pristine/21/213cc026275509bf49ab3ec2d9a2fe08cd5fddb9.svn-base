using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;

using ProfitManager.Web.RepositorioWeb;
using ProfitManager.Web.ViewModels;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    [HandleError]
    public class ContaContabilSaldoInicialController : BaseController
    {

        public ActionResult Index()
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();


            ViewBag.empresaDados = FuncoesEmpresaUsuario.BuscarEmpresa(empresaUsuario.Empresa.Id);
            ViewBag.empresaUsuarioDados = FuncoesEmpresaUsuario.BuscarEmpresaUsuario(empresaUsuario.Empresa.Id);

            FuncoesContaContabilSaldoInicial.DeletarContaContabilSaldo(empresaUsuario.Empresa.Id);
            FuncoesContaContabilSaldoInicial.DeletarContaContabilSaldoInicial(empresaUsuario.Empresa.Id);

            ContaContabilSaldoInicial_Read();

            var listExisteLucroPrejuizo = FuncoesContaContabil.LoadWithSeekByHqlObject().Where(x => x.LucroPrejuizoAcumulado == EnumSimNao.Sim);

            var listExisteSaldoInicial = FuncoesContaContabil.LoadWithSeekByHqlObject().Where(x => x.ExigeSaldoinicial == EnumExigeSaldoinicial.Sim);

            if (listExisteLucroPrejuizo.Count() == 0)
            {
                ViewBag.ExisteLucroPrejuizoAcumulado = 0;
            }
            if (listExisteSaldoInicial.Count() == 0)
            {
                ViewBag.ExisteSaldoinicial = 0;
            }

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
            ViewBag.existeLancamento = FuncoesEventoLancamento.ExisteLancamento(empresaUsuario.Empresa.Id);
            var data = FuncoesContaContabilSaldoInicial.CarregaDataSaldoInicial(empresaUsuario.Empresa.Id);
            ViewBag.dataSaldoInicial = data != null ? data.DataHoraCriacao : DateTime.Now;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContaContabilSaldoInicial_Salvar(FormCollection form)
        {

            var contas = form.AllKeys.Where(x => x.StartsWith("S"));
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(false).Id);


            var listContaContabilSaldo = new List<ContaContabilSaldoInicialViewModel>();

            foreach (var item in contas)
            {
                int idConta = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));
                var listSaldoInicial = FuncoesContaContabilSaldoInicial.LoadById(idConta);

                if (listSaldoInicial.Count != 0)
                {
                    foreach (var idcontacontabil in listSaldoInicial)
                    {
                        var contaLancamento = new ContaContabilSaldoInicialViewModel
                        {
                            IdContaContabil = idcontacontabil.ContaContabil.Id,
                            Id = idConta,
                            Valor = Convert.ToDecimal(form[item])
                        };
                        listContaContabilSaldo.Add(contaLancamento);

                    }
                }
                else
                {
                    var contaLancamento = new ContaContabilSaldoInicialViewModel
                    {
                        IdContaContabil = idConta,
                        Valor = Convert.ToDecimal(form[item])
                    };
                    listContaContabilSaldo.Add(contaLancamento);

                }
            }

            List<ContaContabilSaldoInicial> listContaContabilSaldoInicialBd;
            if (listContaContabilSaldo.All(x => x.Id != 0))
            {
                listContaContabilSaldoInicialBd =
                    FuncoesContaContabilSaldoInicial.Load(listContaContabilSaldo.Select(x => x.Id).ToList());

                foreach (var contaContabilSaldoInicial in listContaContabilSaldoInicialBd)
                {
                    if (listContaContabilSaldo.Any(x => x.Id == contaContabilSaldoInicial.Id))
                    {
                        contaContabilSaldoInicial.Valor = listContaContabilSaldo.First(x => x.Id == contaContabilSaldoInicial.Id).Valor;
                    }
                }
            }
            else
            {
                listContaContabilSaldoInicialBd = listContaContabilSaldo.Select(item => new ContaContabilSaldoInicial
                {
                    Id = item.Id,
                    ContaContabil = new ContaContabil
                    {
                        Id = item.IdContaContabil,
                        CampoNome = new Campo
                        {
                            Nome = item.ContaContabil
                        }

                    },
                    Valor = item.Valor,
                    DataHoraCriacao = item.DataHoraCriacaoContaContabilSaldoIncial,
                    Empresa = new Empresa { Id = empresaUsuario.Empresa.Id }
                }).ToList();
            }

            listContaContabilSaldoInicialBd = FuncoesContaContabilSaldoInicial.Salvar(listContaContabilSaldoInicialBd);


            listContaContabilSaldo.AddRange(listContaContabilSaldoInicialBd.Select(item => new ContaContabilSaldoInicialViewModel
            {
                Id = item.Id,
                Valor = item.Valor,
                IdEmpresa = item.Empresa.Id,
                ContaContabil = item.ContaContabil.CampoNome.Nome,
                IdContaContabil = item.ContaContabil.Id,
                DataHoraCriacaoContaContabilSaldoIncial = item.DataHoraCriacao
            }));

            TempData["mensagem"] = "Sados iniciais gravados!";

            return RedirectToAction("Index");
        }

        [HandleError]
        public ActionResult ContaContabilSaldoInicial_Read()
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            var listContaContabil = FuncoesContaContabil.LoadWithSeekByHqlObject().Where(x => x.ExigeSaldoinicial == EnumExigeSaldoinicial.Sim || x.LucroPrejuizoAcumulado == EnumSimNao.Sim);

            var listContaContabilSaldoInicial = FuncoesContaContabilSaldoInicial.LoadByEmpresa(
                empresaUsuario.Empresa.Id, true);

            foreach (var contaContabil in listContaContabil)
            {
                if (listContaContabilSaldoInicial.All(x => x.ContaContabil.Id != contaContabil.Id))
                {
                    listContaContabilSaldoInicial.Add(new ContaContabilSaldoInicial
                    {
                        ContaContabil = contaContabil,
                        CampoNome = contaContabil.CampoNome,
                        Empresa = empresaUsuario.Empresa,
                        NotaExplicativaContaContabil = contaContabil.NotaExplicativaContaContabil,
                        Id = contaContabil.Id,
                        SubGrupoCodigo = contaContabil.SubGrupoCodigo,
                        SubGrupoNome = contaContabil.SubGrupoNome,
                        GrupoCodigo = contaContabil.GrupoCodigo,
                        GrupoNome = contaContabil.GrupoNome,
                        NotaExplicativaGrupo = contaContabil.NotaExplicativaGrupo,
                        NotaExplicativaSubGrupo = contaContabil.NotaExplicativaSubGrupo,
                        CampoHelp = contaContabil.CampoHelp,
                        Help = contaContabil.Help
                    });
                }
            }

            var listContaContabilSaldoInicialViewModel =
                listContaContabilSaldoInicial.Select(saldoInicial => new ContaContabilSaldoInicialViewModel
                {
                    Id = saldoInicial.Id,
                    Valor = saldoInicial.Valor,
                    IdEmpresa = saldoInicial.Empresa.Id,
                    ContaContabil = saldoInicial.ContaContabil.CampoNome.Nome,
                    IdContaContabil = saldoInicial.ContaContabil.Id,
                    DataHoraCriacaoContaContabilSaldoIncial = saldoInicial.DataHoraCriacao,
                    NotaExplicativaContaContabil = saldoInicial.NotaExplicativaContaContabil,
                    ExigeSaldoinicial = saldoInicial.ExigeSaldoinicial,
                    LucroPrejuizoAcumulado = saldoInicial.LucroPrejuizoAcumulado,
                    SubGrupoCodigo = saldoInicial.SubGrupoCodigo,
                    SubGrupoNome = saldoInicial.SubGrupoNome,
                    GrupoCodigo = saldoInicial.GrupoCodigo,
                    GrupoNome = saldoInicial.GrupoNome,
                    NotaExplicativaGrupo = saldoInicial.NotaExplicativaGrupo,
                    NotaExplicativaSubGrupo = saldoInicial.NotaExplicativaSubGrupo,
                    CampoHelp = saldoInicial.CampoHelp,
                    TipoContaContabil = saldoInicial.TipoContaContabil,
                    Help = saldoInicial.Help
                }).ToList();



            return View(listContaContabilSaldoInicialViewModel);
        }



        [HttpGet]
        public ActionResult AbreContaHelp(int? id)
        {
            var conta = FuncoesContaContabilSaldoInicial.CarregaContaHelp((int)id);
            var nomeConta = FuncoesContaContabil.Load((int)id);
            ViewBag.contaHelp = conta.ContaContabil.Help;
            ViewBag.NomeConta = nomeConta.NotaExplicativaContaContabil;
            return PartialView("AbreContaHelp", conta);
        }
    }
}
