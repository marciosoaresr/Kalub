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

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    [HandleError]
    public class EventosController : BaseController
    {
        public ActionResult Index(int? pageNumber, string search)
        {
            List<Evento> listEventos = (search.HasValue()) ? FuncoesEvento.Load(search): FuncoesEvento.Load();

            listEventos = listEventos.OrderBy(x => x.Id).ToList();

            const int pageSize = 160;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listEventos != null && listEventos.Count > 0 ? (listEventos.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            return View(listEventos.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesEvento.Excluir((int) id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarEvento(int? id)
        {
            var evento = (id.HasValue)? FuncoesEvento.Load((int)id) : new Evento();

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaSubArea = ListSubArea(evento);
            ViewBag.ListaContaContabil = ListContaContabil();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa().OrderBy(x => x.Text);

            return PartialView("CadastrarEditarEvento", evento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CadastrarEditarEvento(Evento evento, FormCollection form)
        {
            LoadBagEvento(form, ref evento);
            LoadBagEventoCategoriaEmpresa(form, ref evento);

            ModelState["Id"].Errors.Clear();
            ModelState["Help"].Errors.Clear();
            ModelState["CampoHelp.Nome"].Errors.Clear();
            ModelState["SubArea.Id"].Errors.Clear();

            if (!string.IsNullOrEmpty(form["adicionar"]))
            {
                return AdicionarEventoOperacao(evento, form);
            }

            if (!string.IsNullOrEmpty(form["excluirItem"]))
            {
                return ExcluirEventoOperacao(evento, form);
            }

            if (!string.IsNullOrEmpty(form["adicionarCategoria"]))
            {
                return AdicionarEventoCategoria(evento, form);
            }

            if (!string.IsNullOrEmpty(form["excluirItemCategoria"]))
            {
                return ExcluirEventoCategoria(evento, form);
            }
            if (evento.Id != 246)
            {
                evento.ApuracaoResultado = EnumSimNao.Nao;
            }
            else
            {
                evento.ApuracaoResultado = EnumSimNao.Sim;
            }

            //var help = FuncoesEvento.Load(evento.Id);
            //if (help != null)
            //{
            //    help.CampoNome = evento.CampoNome;
            //    help.ApuracaoResultado = evento.ApuracaoResultado;
            //    help.Area = evento.Area;
            //    help.SubArea = evento.SubArea;
            //    help.CampoHelp = evento.CampoHelp;
            //    help.Codigo = evento.Codigo;
            //    help.MaisUsado = evento.MaisUsado;
            //    help.RestringeCategoriaEmpresa = evento.RestringeCategoriaEmpresa;
            //    help.TipoDFC = evento.TipoDFC;
            //    help.TipoEventoLancamento = evento.TipoEventoLancamento;
            //    help.Id = evento.Id;
            //    help.Ordem = evento.Ordem;
            //    if (help.SubArea.Id == 0)
            //    {
            //        help.SubArea = null;
            //    }
            //    foreach (var eventoOperacao in evento.ListEventoOperacao)
            //    {
            //        help.AddListEventoOperacao(eventoOperacao);
            //    }
            //    foreach (var eventoCategoria in evento.ListEventoCategoriaEmpresa)
            //    {
            //        help.AddListEventoCategoria(eventoCategoria);
            //    }
            //}
            if (ModelState.IsValid)
            {
                //if (help != null)
                //{
                //    evento.Help = help.Help;
                //    FuncoesEvento.Salvar(help);
                //}
                //else
                //{
                if (evento.SubArea.Id == 0)
                {
                    evento.SubArea = null;
                }
                if (evento.Help == null)
                {
                    evento.Help = "help";
                }
                FuncoesEvento.Salvar(evento);
                //}
            return RedirectToAction("Index");
            }

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaContaContabil = ListContaContabil();
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa().OrderBy(x => x.Text);

            return View(evento);
        }

        [HttpGet]
        public ActionResult CadastrarEditarEventoHelp(int? id)
        {
            var evento = (id.HasValue) ? FuncoesEvento.Load((int)id) : new Evento();

            return PartialView("CadastrarEditarEventoHelp", evento);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadastrarEditarEventoHelp(Evento eventoHelp, FormCollection form)
        {
            ModelState["Area"].Errors.Clear();
            ModelState["SubArea"].Errors.Clear();
            ModelState["Codigo"].Errors.Clear();
            ModelState["CampoNome"].Errors.Clear();
            ModelState["CampoHelp"].Errors.Clear();
            var evento = FuncoesEvento.Load(eventoHelp.Id);
            if (evento != null)
            {
                eventoHelp.CampoNome = evento.CampoNome;
                eventoHelp.ApuracaoResultado = evento.ApuracaoResultado;
                eventoHelp.Area = evento.Area;
                eventoHelp.SubArea = evento.SubArea;
                eventoHelp.CampoHelp = evento.CampoHelp;
                eventoHelp.Codigo = evento.Codigo;
                eventoHelp.MaisUsado = evento.MaisUsado;
                eventoHelp.RestringeCategoriaEmpresa = evento.RestringeCategoriaEmpresa;
                eventoHelp.TipoDFC = evento.TipoDFC;
                eventoHelp.TipoEventoLancamento = evento.TipoEventoLancamento;
                eventoHelp.Id = evento.Id;
                eventoHelp.Ordem = evento.Ordem;
                //if (eventoHelp.SubArea.Id == 0)
                //{
                    eventoHelp.SubArea = evento.SubArea;
                //}
                foreach (var eventoOperacao in evento.ListEventoOperacao)
                {
                    eventoHelp.AddListEventoOperacao(eventoOperacao);
                }
                foreach (var eventoCategoria in evento.ListEventoCategoriaEmpresa)
                {
                    eventoHelp.AddListEventoCategoria(eventoCategoria);
                }
            }

            if (ModelState.IsValid)
            {
                //if (evento != null)
                //{
                //    evento.Help = eventoHelp.Help;
                //    FuncoesEvento.Salvar(evento);
                //}
                //else
                //{
                    FuncoesEvento.Salvar(eventoHelp);
                //}
                return RedirectToAction("Index");
            }

            return View(eventoHelp);
        }

        [HttpGet]
        public ActionResult EventoRecalcularSaldo(int? id)
        {
            var evento = (id.HasValue) ? FuncoesEvento.Load((int)id) : new Evento();

            return PartialView("EventoRecalcularSaldo", evento);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EventoRecalcularSaldo(Evento evento, FormCollection form)
        {

                FuncoesEventoLancamento.RecalcularSaldoEvento(evento.Id);
                return RedirectToAction("Index");

        }

        private ActionResult ExcluirEventoOperacao(Evento evento, FormCollection form)
        {
            int hashCodeEventoOperacao = Convert.ToInt32(form["excluirItem"]);
            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            evento.RemoveItemListEventoOperacao(hashCodeEventoOperacao);

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaContaContabil = ListContaContabil();
            

            return PartialView("EventoOperacao", evento);
        }

        private ActionResult ExcluirEventoCategoria(Evento evento, FormCollection form)
        {
            int hashCodeEventoCategoria = Convert.ToInt32(form["excluirItemCategoria"]);
            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            evento.RemoveItemListEventoCategoria(hashCodeEventoCategoria);

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa();


            return PartialView("EventoCategoriaEmpresa", evento);
        }

        public ActionResult AdicionarEventoOperacao(Evento evento, FormCollection form)
        {
            var tipoOperacao = form["tipoOperacao"];
            var idConta = form["nameConta"];

            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            if ((!string.IsNullOrEmpty(tipoOperacao)) && (!string.IsNullOrEmpty(idConta)))
            {
                var eventoOperacao = new EventoOperacao();
                if ((EnumEventoOperacaoTipo)Convert.ToChar(tipoOperacao) == EnumEventoOperacaoTipo.Debitar)
                {
                    eventoOperacao.Tipo = EnumEventoOperacaoTipo.Debitar;
                }
                else if ((EnumEventoOperacaoTipo)Convert.ToChar(tipoOperacao) == EnumEventoOperacaoTipo.Creditar)
                {
                    eventoOperacao.Tipo = EnumEventoOperacaoTipo.Creditar;
                }

                eventoOperacao.ContaContabil = FuncoesContaContabil.Load(Convert.ToInt32(idConta));
                evento.AddListEventoOperacao(eventoOperacao);
            }

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaContaContabil = ListContaContabil();
            ViewBag.OpenModal = true;


            return PartialView("EventoOperacao", evento);
        }

        public ActionResult AdicionarEventoCategoria(Evento evento, FormCollection form)
        {
            var idConta = form["categoriaEmpresa"];

            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            if (!string.IsNullOrEmpty(idConta))
            {
                var eventoCategoria = new EventoCategoriaEmpresa();

                eventoCategoria.CategoriaEmpresa = FuncoesCategoriaEmpresa.Load(Convert.ToInt32(idConta));
                evento.AddListEventoCategoria(eventoCategoria);
            }

            ViewBag.ListaArea = ListArea(evento);
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa();
            ViewBag.OpenModal = true;


            return PartialView("EventoCategoriaEmpresa", evento);
        }

        [HttpGet]
        public JsonResult AdicionarEventoOperacao(Campo campo)
        {
            int idContaContabil = Convert.ToInt32(campo.Id);
            var tipoOperacao = (EnumEventoOperacaoTipo)Convert.ToChar(campo.Codigo);

            var eventoOperacao = new EventoOperacao();
            eventoOperacao.Tipo = tipoOperacao;
            eventoOperacao.ContaContabil = FuncoesContaContabil.Load(idContaContabil);

            return Json(new { Result = eventoOperacao }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AdicionarEventoCategoria(Campo campo)
        {
            int idEmpresaCategoria = Convert.ToInt32(campo.Id);

            var eventoCategoria = new EventoCategoriaEmpresa();
            eventoCategoria.CategoriaEmpresa = FuncoesCategoriaEmpresa.Load(idEmpresaCategoria);

            return Json(new { Result = eventoCategoria }, JsonRequestBehavior.AllowGet);
        }
        private List<SelectListItem> ListArea(Evento evento)
        {
            List<Area> listArea = FuncoesArea.Load();
            var listItemsArea = new List<SelectListItem>();

            foreach (var area in listArea)
            {
                var item = new SelectListItem
                {
                    Text = area.NomeNormalizado,
                    Value = area.Id.ToString()
                };

                if (evento.Area != null && area.Id == evento.Area.Id)
                {
                    item.Selected = true;
                }

                listItemsArea.Add(item);
            }

            return listItemsArea;
        }

        private List<SelectListItem> ListSubArea(Evento evento)
        {
            List<SubArea> listSubArea = FuncoesSubArea.Load();
            var listItemsSubArea = new List<SelectListItem>();

            foreach (var subarea in listSubArea)
            {
                var item = new SelectListItem
                {
                    Text = subarea.NomeNormalizado,
                    Value = subarea.Id.ToString()
                };

                if (evento.SubArea != null && subarea.Id == evento.SubArea.Id)
                {
                    item.Selected = true;
                }

                listItemsSubArea.Add(item);
            }

            return listItemsSubArea;
        }

        private List<SelectListItem> ListContaContabil()
        {
            List<ContaContabil> listContaContabil = FuncoesContaContabil.Load();
            var listItemsContaContabil = new List<SelectListItem>();

            foreach (var conta in listContaContabil)
            {
                var item = new SelectListItem
                {
                    Text = conta.NomeNormalizado,
                    Value = conta.Id.ToString()
                };

                listItemsContaContabil.Add(item);
            }

            return listItemsContaContabil;
        }

        private List<SelectListItem> ListCategoriaEmpresa()
        {
            List<CategoriaEmpresa> listCategoriaEmpresa = FuncoesCategoriaEmpresa.Load();
            var listItemsCategoriaEmpresa = new List<SelectListItem>();

            foreach (var conta in listCategoriaEmpresa)
            {
                var item = new SelectListItem
                {
                    Text = conta.CampoNome.Nome,
                    Value = conta.Id.ToString()
                };

                listItemsCategoriaEmpresa.Add(item);
            }

            return listItemsCategoriaEmpresa;
        }

        private static void LoadBagEvento(FormCollection form, ref Evento evento)
        {
            var dictionaryBagEventoOperacao = new Dictionary<KeyValuePair<int, string>, object>();

            if (form.AllKeys.Any(x => x.StartsWith("ListEventoOperacao")))
            {
                foreach (var item in form.AllKeys.Where(x => x.StartsWith("ListEventoOperacao"))) 
                {
                    if (item.StartsWith("ListEventoOperacao"))
                    {
                        int indexElement = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));
                        int indexPonto = item.IndexOf('.') + 1;
                        string property = item.Substring(indexPonto, item.Length - indexPonto);

                        dictionaryBagEventoOperacao.Add(new KeyValuePair<int, string>(indexElement, property),
                            form.GetValue(item).AttemptedValue);
                    }
                }

                int maxIndex = dictionaryBagEventoOperacao.Max(x => x.Key.Key) + 1;

                int posicaoList = 0;

                while (posicaoList != maxIndex)
                {
                    int list = posicaoList;

                    var dictionaryItensMesmaPosicao =
                        dictionaryBagEventoOperacao.Where(x => x.Key.Key == list).GroupBy(x => x.Key.Key);

                    foreach (var grupoProperyMesmaPosicao in dictionaryItensMesmaPosicao)
                    {
                        var eventoOperacao = new EventoOperacao();
                        foreach (var property in grupoProperyMesmaPosicao)
                        {
                            object value = property.Value;

                            if (property.Key.Value == "Id" && Convert.ToInt32(value) != 0)
                            {
                                eventoOperacao = FuncoesEventoOperacao.Load(Convert.ToInt32(value));
                                break;
                            }
                            else
                            {
                                if (property.Key.Value == "ContaContabil.Id")
                                {
                                    eventoOperacao.ContaContabil = FuncoesContaContabil.Load(Convert.ToInt32(value));
                                }
                                else if (property.Key.Value == "Tipo")
                                {
                                    eventoOperacao.Tipo =
                                        (EnumEventoOperacaoTipo)Convert.ToChar(value.ToString().Substring(0, 1));
                                }
                            }
                        }

                        if (eventoOperacao != null) evento.AddListEventoOperacao(eventoOperacao);

                        posicaoList++;
                    }

                }
            }
        }

        private static void LoadBagEventoCategoriaEmpresa(FormCollection form, ref Evento evento)
        {
            var dictionaryBagEventoCategoriaEmpresa = new Dictionary<KeyValuePair<int, string>, object>();

            if (form.AllKeys.Any(x => x.StartsWith("ListEventoCategoriaEmpresa")))
            {
                foreach (var item in form.AllKeys.Where(x => x.StartsWith("ListEventoCategoriaEmpresa")))
                {
                    if (item.StartsWith("ListEventoCategoriaEmpresa"))
                    {
                        int indexElement = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));
                        int indexPonto = item.IndexOf('.') + 1;
                        string property = item.Substring(indexPonto, item.Length - indexPonto);

                        dictionaryBagEventoCategoriaEmpresa.Add(new KeyValuePair<int, string>(indexElement, property),
                            form.GetValue(item).AttemptedValue);
                    }
                }

                int maxIndex = dictionaryBagEventoCategoriaEmpresa.Max(x => x.Key.Key) + 1;

                int posicaoList = 0;

                while (posicaoList != maxIndex)
                {
                    int list = posicaoList;

                    var dictionaryItensMesmaPosicao =
                        dictionaryBagEventoCategoriaEmpresa.Where(x => x.Key.Key == list).GroupBy(x => x.Key.Key);

                    foreach (var grupoProperyMesmaPosicao in dictionaryItensMesmaPosicao)
                    {
                        var eventoCategoriaEmpresa = new EventoCategoriaEmpresa();
                        foreach (var property in grupoProperyMesmaPosicao)
                        {
                            object value = property.Value;

                            if (property.Key.Value == "Id" && Convert.ToInt32(value) != 0)
                            {
                                eventoCategoriaEmpresa = FuncoesEventoCategoriaEmpresa.Load(Convert.ToInt32(value));
                                break;
                            }
                            else
                            {
                                if (property.Key.Value == "CategoriaEmpresa.Id")
                                {
                                    eventoCategoriaEmpresa.CategoriaEmpresa = FuncoesCategoriaEmpresa.Load(Convert.ToInt32(value));
                                }
                            }
                        }

                        evento.AddListEventoCategoria(eventoCategoriaEmpresa);

                        posicaoList++;
                    }

                }
            }
        }


        [HttpPost]
        public JsonResult Evento_Read([DataSourceRequest]DataSourceRequest request)
        {
            var eventos = GetEventos();

            var enumerable = eventos as Evento[] ?? eventos.ToArray();
            foreach (var evento in enumerable)
            {
                foreach (var eventoOperacao in evento.ListEventoOperacao)
                {
                    eventoOperacao.Evento = null;
                    eventoOperacao.ContaContabil = null;
                }
                foreach (var eventoCategoria in evento.ListEventoCategoriaEmpresa)
                {
                    eventoCategoria.Evento = null;
                    eventoCategoria.CategoriaEmpresa = null;
                }
            }
            DataSourceResult result = enumerable.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Evento> GetEventos()
        {
            return FuncoesEvento.Load();
        }


    }
}
