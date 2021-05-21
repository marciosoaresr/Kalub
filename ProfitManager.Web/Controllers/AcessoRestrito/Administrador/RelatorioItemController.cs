using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kendo.Mvc.Extensions;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class RelatorioItemController : BaseController
    {
        public ActionResult Index(int? pageNumber, string search)
        {
            var listRelatorioItem = (search.HasValue())
                ? FuncoesRelatorioItem.Load(search)
                : FuncoesRelatorioItem.Load();

            const int pageSize = 10;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listRelatorioItem != null && listRelatorioItem.Count > 0
                ? (listRelatorioItem.Count + pageSize - 1)/pageSize
                : 1;
            ViewBag.PageNumber = pageCurrent;

            return View(listRelatorioItem.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult CadastrarEditarRelatorioItem(int? idRelatorioItem)
        {
            var item = (idRelatorioItem.HasValue)
                ? FuncoesRelatorioItem.Load((int) idRelatorioItem)
                : new RelatorioItem();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarRelatorioItem(RelatorioItem relatorioItem, FormCollection form)
        {
            string actionForm = "";
            if (!string.IsNullOrEmpty(form["adicionar"]))
                actionForm = "AddItemBag";
            else if (!string.IsNullOrEmpty(form["excluirItem"]))
                actionForm = "RemoveItemBag";
            else
                actionForm = "SaveItem";
            
            LoadBagContaContabilRelatorioItem(form, ref relatorioItem);

            if (!string.IsNullOrEmpty(form["Relatorio.Id"]))
            {
                relatorioItem.Relatorio = FuncoesRelatorio.Load(Convert.ToInt32(form["Relatorio.Id"]));
                var item = ModelState.First(x => x.Key == ("Relatorio"));
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            relatorioItem.Parent = !string.IsNullOrEmpty(form["Parent.Id"])
                ? FuncoesRelatorioItem.Load(Convert.ToInt32(form["Parent.Id"]))
                : null;

            if (actionForm == "AddItemBag")
            {
                return AdicionarContaContabilRelatorioItem(relatorioItem, form);
            }
            if (actionForm == "RemoveItemBag")
            {
                return ExcluirContaContabilRelatorioItem(relatorioItem, form);
            }

            while (ModelState.Keys.Any(x => x.StartsWith("Parent")))
            {
                var item = ModelState.First(x => x.Key.StartsWith("Parent"));
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value)); 
            }

            if (ModelState.IsValid)
            {
                FuncoesRelatorioItem.Salvar(relatorioItem);

                return RedirectToAction("Index");
            }


            return View(relatorioItem);
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesRelatorioItem.Excluir((int)id);

            return RedirectToAction("Index");
        }

        private ActionResult ExcluirContaContabilRelatorioItem(RelatorioItem relatorioItem, FormCollection form)
        {
            int hashContaContabilRelatorioItem = Convert.ToInt32(form["excluirItem"]);
            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            relatorioItem.RemoveItemListContaContabilRelatorioItem(hashContaContabilRelatorioItem);

            return View("CadastrarEditarRelatorioItem", relatorioItem);
        }

        public JsonResult GetContaContabil(string text)
        {
            var listContaContabil = new List<ContaContabil>();

            if (!string.IsNullOrEmpty(text.Trim()))
            {
                listContaContabil = FuncoesContaContabil.Load(text);
            }

            var listReturnContaContabil = listContaContabil.Select(contaContabil => new ContaContabil
            {
                CampoNome = new Campo
                {
                    Nome = contaContabil.CampoNome.Nome
                },
                Id = contaContabil.Id, Codigo = contaContabil.Codigo
            }).ToList();

            return Json(listReturnContaContabil, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRelatorio(string text)
        {
           var listRelatorio = new List<Relatorio>();

            if (!string.IsNullOrEmpty(text.Trim()))
            {
                listRelatorio = FuncoesRelatorio.Load(text);
            }

            return Json(listRelatorio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRelatorioItem(string text, int idRelatorio)
        {
            var listRelatorioItem = new List<RelatorioItem>();

            if (!string.IsNullOrEmpty(text.Trim()))
            {
                listRelatorioItem = FuncoesRelatorioItem.Load(text, idRelatorio);
            }

            var listReturnRelatorioItem = listRelatorioItem.Select(relItem => new RelatorioItem
            {
                Id = relItem.Id,
                Codigo = relItem.Codigo,
                Nome = relItem.Nome
            });

            return Json(listReturnRelatorioItem, JsonRequestBehavior.AllowGet);
        }

        private static void LoadBagContaContabilRelatorioItem(FormCollection form, ref RelatorioItem relatorioItem)
        {
            var dictionaryBagContaContabilRelatorioItem = new Dictionary<KeyValuePair<int, string>, object>();

            if (form.AllKeys.Any(x => x.StartsWith("ListContaContabilRelatorioItem")))
            {
                foreach (var item in form.AllKeys.Where(x => x.StartsWith("ListContaContabilRelatorioItem"))) 
                {
                    if (item.StartsWith("ListContaContabilRelatorioItem"))
                    {
                        int indexElement = Convert.ToInt32(String.Join("", Regex.Split(item, @"[^\d]")));
                        int indexPonto = item.IndexOf('.') + 1;
                        string property = item.Substring(indexPonto, item.Length - indexPonto);

                        dictionaryBagContaContabilRelatorioItem.Add(new KeyValuePair<int, string>(indexElement, property),
                            form.GetValue(item).AttemptedValue);
                    }
                }

                int maxIndex = dictionaryBagContaContabilRelatorioItem.Max(x => x.Key.Key) + 1;

                int posicaoList = 0;

                while (posicaoList != maxIndex)
                {
                    int list = posicaoList;

                    var dictionaryItensMesmaPosicao =
                        dictionaryBagContaContabilRelatorioItem.Where(x => x.Key.Key == list).GroupBy(x => x.Key.Key);

                    foreach (var grupoProperyMesmaPosicao in dictionaryItensMesmaPosicao)
                    {
                        var contaContabilRelatorioItem = new ContaContabilRelatorioItem();

                        foreach (var property in grupoProperyMesmaPosicao)
                        {
                            object value = property.Value;

                            if (property.Key.Value == "Id" && Convert.ToInt32(value) != 0)
                            {
                                contaContabilRelatorioItem =
                                    FuncoesContaContabilRelatorioItem.Load(Convert.ToInt32(value));
                                    
                                break;
                            }
                            else
                            {
                                if (property.Key.Value == "ContaContabil.Id")
                                {
                                    contaContabilRelatorioItem.ContaContabil =
                                        FuncoesContaContabil.Load(Convert.ToInt32(value));
                                }
                               
                            }
                        }

                        relatorioItem.AddListContaContabilRelatorioItem(contaContabilRelatorioItem);

                        posicaoList++;
                    }

                }
            }
        }

        public ActionResult AdicionarContaContabilRelatorioItem(RelatorioItem relItem, FormCollection form)
        {
            var idConta = form["idContaContabil"];

            while (ModelState.Any())
            {
                var item = ModelState.First();
                ModelState.Remove(new KeyValuePair<string, ModelState>(item.Key, item.Value));
            }

            if (!string.IsNullOrEmpty(idConta))
            {
                var contaContabilRelatorioItem = new ContaContabilRelatorioItem();

                contaContabilRelatorioItem.ContaContabil = FuncoesContaContabil.Load(Convert.ToInt32(idConta));

                relItem.AddListContaContabilRelatorioItem(contaContabilRelatorioItem);
            }       

            return View("CadastrarEditarRelatorioItem", relItem);
        }

    }
}
