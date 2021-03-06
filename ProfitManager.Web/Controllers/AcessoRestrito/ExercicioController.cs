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
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    public class ExercicioController : Controller
    {
        public ActionResult Index(int? pageNumber, string search, bool? erro)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();

            List<ExercicioEmpresa> listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();

            var listExercicios = new List<Exercicio>();

            foreach (var exercicioempresa in listExercicioEmpresa)
            {
                listExercicios.Add(exercicioempresa.Exercicio);
            }


            const int pageSize = 160;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listExercicios != null && listExercicios.Count > 0 ? (listExercicios.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;


            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.CategoriaEmpresa = empresaUsuario.Empresa.CategoriaEmpresa.Id;

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
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            bool acessoExercicio = listExercicioEmpresa.Count > 0;
            ViewBag.existeExercicio = false;
            ViewBag.tipoCadastroProfessor = tipo;

            if (tipo == EnumTipoCadastro.Professor && acessoExercicio)
            {
                ViewBag.existeExercicio = true;
            }

            return View(listExercicios.ToPagedList(pageCurrent, pageSize));
        }

        public ActionResult CompartilharExercicio(int? pageNumber, string search)
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();

            List<Exercicio> listExercicios = (search.HasValue()) ? FuncoesExercicio.Load(search) : FuncoesExercicio.Load();

            ViewBag.listExercicios = listExercicios.Where(x => x.Empresa.Id != empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();

            //const int pageSize = 160;
            //int pageCurrent = (pageNumber ?? 1);
            //ViewBag.PageCount = listExercicios != null && listExercicios.Count > 0 ? (listExercicios.Count + pageSize - 1) / pageSize : 1;
            //ViewBag.PageNumber = pageCurrent;
            //ViewBag.Search = search;


            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.CategoriaEmpresa = empresaUsuario.Empresa.CategoriaEmpresa.Id;

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
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            bool acessoExercicio = listExercicios.Count > 0;
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
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            var listExercicios = FuncoesExercicio.Load((int)id);
            if (listExercicios.Empresa.Id == empresaUsuario.Empresa.Id)
            {
                if (id.HasValue)
                    FuncoesExercicioItem.DeletarExercicioItem((int)id);
                FuncoesExercicioEmpresa.DeletarExercicioEmpresa((int)id);
                FuncoesExercicio.Excluir((int)id);
            }
            else
            {
                TempData["erro"] = "Você não tem permissão para excluir o Exercicio!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarExercicio(int? id)
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            if (id != null)
            {
                var listExercicios = FuncoesExercicio.Load((int)id);
                if (listExercicios.Empresa.Id != empresaUsuario.Empresa.Id)
                {
                    TempData["erro"] = "Você não tem permissão para editar o Exercicio!";
                }
            }

            var exercicio = (id.HasValue) ? FuncoesExercicio.Load((int)id) : new Exercicio();

            if (id != null)
            {
                var exercicioItem = FuncoesExercicioItem.LoadExercicioItem((int)id);
                foreach (var exercicioOperacao in exercicioItem)
                {
                    exercicio.AddListExercicioItem(exercicioOperacao);
                }
            }

            //ViewBag.ListaEvento = ListEvento();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(null).OrderBy(x => x.Text);

            return PartialView("CadastrarEditarExercicio", exercicio);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CadastrarEditarExercicio(Exercicio exercicio, FormCollection form)
        {

            ModelState["Id"].Errors.Clear();
            ModelState["CategoriaEmpresa.Codigo"].Errors.Clear();
            ModelState["CategoriaEmpresa.CampoNome"].Errors.Clear();

            //LoadBagExercicio(form, ref exercicio);
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);
            exercicio.Empresa = empresaUsuario.Empresa;

            var dataFinal = exercicio.DataInicio.AddMonths(6);
            exercicio.DataFim = dataFinal;

            //if (!string.IsNullOrEmpty(form["adicionar"]))
            //{
                //return AdicionarExercicioOperacao(exercicio, form);
            //}
            //if (!string.IsNullOrEmpty(form["excluirItem"]))
            //{
                //return ExcluirExercicioOperacao(exercicio, form);
            //}

            if (ModelState.IsValid)
            {
                exercicio.Usuario = empresaUsuario.Empresa.Nome;
                FuncoesExercicio.Salvar(exercicio);

                var listExercicioEmpresa = FuncoesExercicioEmpresa.Load(exercicio.Id);
                if (listExercicioEmpresa == null)
                {
                    var exercicioempresa = new ExercicioEmpresa();
                    exercicioempresa.Empresa = empresaUsuario.Empresa;
                    exercicioempresa.Exercicio = exercicio;
                    FuncoesExercicioEmpresa.Salvar(exercicioempresa);
                }

                //var listExercicioEmpresa0 = FuncoesExercicioEmpresa.Load(exercicio.Id);
                //if (listExercicioEmpresa0.Exercicio.Id != exercicio.Id)
                //{
                //var exercicioempresa = new ExercicioEmpresa();
                //exercicioempresa.Empresa = empresaUsuario.Empresa;
                //exercicioempresa.Exercicio = exercicio;
                //FuncoesExercicioEmpresa.Salvar(exercicioempresa);
                //}


                //return RedirectToAction("Index");
                return RedirectToAction("Index", "EventoLancamento");
            }

            //ViewBag.ListaEvento = ListEvento();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(null).OrderBy(x => x.Text);

            return View(exercicio);
        }


        [HttpGet]
        public ActionResult CompartilharExercicioItem(int? id)
        {
            //var exercicio = (id.HasValue) ? FuncoesExercicio.Load((int)id) : new Exercicio();

            //if (id != null)
            //{
                //var exercicioItem = FuncoesExercicioItem.LoadExercicioItem((int)id);
                //foreach (var exercicioOperacao in exercicioItem)
                //{
                    //exercicio.AddListExercicioItem(exercicioOperacao);
                //}
            //}

            //ViewBag.ListaEvento = ListEvento();

            return PartialView("CompartilharExercicioItem", null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CompartilharExercicioItem(Exercicio exercicio, FormCollection form)
        {

            //ModelState["Id"].Errors.Clear();

            //EmpresaUsuario empresaUsuario =
                //FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);


            //if (ModelState.IsValid)
            //{

                //var exercicioempresa = new ExercicioEmpresa();
                //exercicioempresa.Empresa = empresaUsuario.Empresa;
                //exercicioempresa.Exercicio = exercicio;
                //FuncoesExercicioEmpresa.Salvar(exercicioempresa);

                //return RedirectToAction("Index");
            //}

            return View(exercicio);
        }
        private List<SelectListItem> ListEvento()
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            List<Evento> listEvento = FuncoesEvento.LoadEventosPorTipo(empresaUsuario.Empresa.CategoriaEmpresa.Id);
            var listItemsEvento = new List<SelectListItem>();

            foreach (var evento in listEvento)
            {
                var item = new SelectListItem
                {
                    Text = evento.CampoNome.Nome,
                    Value = evento.Id.ToString()
                };

                listItemsEvento.Add(item);
            }

            return listItemsEvento;
        }

        [HttpGet]
        public JsonResult AdicionarExercicioOperacao(Campo campo)
        {
            int idEvento = Convert.ToInt32(campo.Id);

            var exercicioOperacao = new ExercicioItem();
            exercicioOperacao.Evento = FuncoesEvento.Load(idEvento);

            return Json(new { Result = exercicioOperacao }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarExercicioOperacao(Exercicio exercicio, FormCollection form)
        {
            var descricao = form["DescricaoItem"];
            var idCategoria = form["nomeCategoria"];
            //var valor = form["Valor"];

            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            if ((!string.IsNullOrEmpty(idCategoria)))
            {
                var exercicioOperacao = new ExercicioItem();

                exercicioOperacao.Descricao = descricao;
                exercicioOperacao.CategoriaEmpresa = FuncoesCategoriaEmpresa.Load(Convert.ToInt32(idCategoria));
                exercicioOperacao.Data = DateTime.Now;
                //exercicioOperacao.Valor = Convert.ToDecimal(valor);
                exercicio.AddListExercicioItem(exercicioOperacao);
            }

            //ViewBag.ListaEvento = ListEvento();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(null).OrderBy(x => x.Text);
            ViewBag.OpenModal = true;


            return PartialView("ExercicioOperacao", exercicio);
        }

        private ActionResult ExcluirExercicioOperacao(Exercicio exercicio, FormCollection form)
        {
            int hashCodeExercicioOperacao = Convert.ToInt32(form["excluirItem"]);
            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            exercicio.RemoveItemListExercicioOperacao(hashCodeExercicioOperacao);

            //ViewBag.ListaEvento = ListEvento();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(null).OrderBy(x => x.Text);

            return PartialView("ExercicioOperacao", exercicio);
        }

        private static void LoadBagExercicio(FormCollection form, ref Exercicio exercicio)
        {
            var dictionaryBagExercicioOperacao = new Dictionary<KeyValuePair<int, string>, object>();

            if (form.AllKeys.Any(x => x.StartsWith("ListExercicioItem")))
            {
                foreach (var item in form.AllKeys.Where(x => x.StartsWith("ListExercicioItem")))
                {
                    if (item.StartsWith("ListExercicioItem"))
                    {
                        int indexElement = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));
                        int indexPonto = item.IndexOf('.') + 1;
                        string property = item.Substring(indexPonto, item.Length - indexPonto);

                        dictionaryBagExercicioOperacao.Add(new KeyValuePair<int, string>(indexElement, property),
                            form.GetValue(item).AttemptedValue);
                    }
                }

                int maxIndex = dictionaryBagExercicioOperacao.Max(x => x.Key.Key) + 1;

                int posicaoList = 0;

                while (posicaoList != maxIndex)
                {
                    int list = posicaoList;

                    var dictionaryItensMesmaPosicao = dictionaryBagExercicioOperacao.Where(x => x.Key.Key == list).GroupBy(x => x.Key.Key);

                    foreach (var grupoProperyMesmaPosicao in dictionaryItensMesmaPosicao)
                    {
                        var exercicioOperacao = new ExercicioItem();
                        foreach (var property in grupoProperyMesmaPosicao)
                        {
                            object value = property.Value;

                            if (property.Key.Value == "Id" && Convert.ToInt32(value) != 0)
                            {
                                exercicioOperacao = FuncoesExercicioItem.Load(Convert.ToInt32(value));
                                break;
                            }
                            else
                            {

                                if (property.Key.Value == "CategoriaEmpresa.Id")
                                {
                                    exercicioOperacao.CategoriaEmpresa = FuncoesCategoriaEmpresa.Load(Convert.ToInt32(value));
                                }
                                else if (property.Key.Value == "Descricao")
                                {
                                    exercicioOperacao.Descricao = value.ToString();
                                }
                                else if (property.Key.Value == "Data")
                                {
                                    exercicioOperacao.Data = Convert.ToDateTime(DateTime.Now);
                                }
                            }
                        }

                        if (exercicioOperacao != null) exercicio.AddListExercicioItem(exercicioOperacao);

                        posicaoList++;
                    }

                }
            }
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


        private List<SelectListItem> ListCategoriaEmpresa(Empresa empresa)
        {
            List<CategoriaEmpresa> listCategoriaEmpresa = FuncoesCategoriaEmpresa.Load();
            var listItemsCategoriaEmpresa = new List<SelectListItem>();

            foreach (var categoriaempresa in listCategoriaEmpresa)
            {
                var item = new SelectListItem
                {
                    Text = categoriaempresa.CampoNome.Nome,
                    Value = categoriaempresa.Id.ToString()
                };

                listItemsCategoriaEmpresa.Add(item);
            }

            return listItemsCategoriaEmpresa.ToList();
        }

        public ActionResult VisualizarExercicio(int id)
        {

            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            var listExercicios = FuncoesExercicio.Load(id);
            ViewBag.Titulo = listExercicios.Titulo;
            ViewBag.DataInicio = listExercicios.DataInicio;
            ViewBag.DataFim = listExercicios.DataFim;
            ViewBag.Descricao = listExercicios.Descricao;
            var nomeCategoria = FuncoesCategoriaEmpresa.Load(listExercicios.CategoriaEmpresa.Id);
            ViewBag.NomeCategoria = nomeCategoria.CampoNome.Nome;
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;

            return View();
        }
    }
}
