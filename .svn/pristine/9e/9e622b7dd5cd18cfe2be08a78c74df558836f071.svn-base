using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;
using ProfitManager.Web.ViewModels;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    [HandleError]
    public class PlanoContasController : BaseController
    {
        public ActionResult Index()
        {
            List<ContaContabilGrupo> listContaContabilGrupo = FuncoesContaContabilGrupo.Load();


            return View(listContaContabilGrupo);
        }

        #region Grupo 

        [HttpGet]
        public ActionResult ExcluirContaContabilGrupo(int? id)
        {
            if (id.HasValue)
                FuncoesContaContabilGrupo.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarContaContabilGrupo(int? id)
        {
            ContaContabilGrupo contaContabilGrupo = id.HasValue
                ? FuncoesContaContabilGrupo.Load((int) id)
                : new ContaContabilGrupo();

            return PartialView("CadastrarEditarContaContabilGrupo", contaContabilGrupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarContaContabilGrupo(ContaContabilGrupo contaContabilGrupo)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesContaContabilGrupo.Salvar(contaContabilGrupo);
                return RedirectToAction("Index");
            }

            return View();
        }

        #endregion

        #region SubGrupo

        [HttpGet]
        public ActionResult ExcluirContaContabilSubGrupo(int? id)
        {
            if(id.HasValue)
                FuncoesContaContabilSubGrupo.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarContaContabilSubGrupo(int? id)
        {
            ContaContabilSubGrupo contaContabilSubGrupo = (id.HasValue)
                ? FuncoesContaContabilSubGrupo.Load((int) id)
                : new ContaContabilSubGrupo();


            List<ContaContabilGrupo> listContaContabilGrupo = FuncoesContaContabilGrupo.Load();

            var listItems = new List<SelectListItem>();
           
            foreach (var contaContabilGrupo in listContaContabilGrupo)
            {
                var item = new SelectListItem
                {
                    Text = contaContabilGrupo.NomeNormalizado,
                    Value = contaContabilGrupo.Id.ToString()
                };

                if (id.HasValue && contaContabilGrupo.Id == contaContabilSubGrupo.ContaContabilGrupo.Id)
                {
                    item.Selected = true;
                }

                listItems.Add(item);
            }

            
            ViewBag.ListaContaContabilGrupo = listItems;

            return PartialView("CadastrarEditarContaContabilSubGrupo",contaContabilSubGrupo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarContaContabilSubGrupo(ContaContabilSubGrupo contaContabilSubGrupo)
        {
            ModelState["ContaContabilGrupo.Codigo"].Errors.Clear();
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesContaContabilSubGrupo.Salvar(contaContabilSubGrupo);
                Thread.Sleep(2000);
                return RedirectToAction("Index");
            }

            List<ContaContabilGrupo> listContaContabilGrupo = FuncoesContaContabilGrupo.Load();

            var listItems = new List<SelectListItem>();

            foreach (var contaContabilGrupo in listContaContabilGrupo)
            {
                listItems.Add(new SelectListItem
                {
                    Text = contaContabilGrupo.NomeNormalizado,
                    Value = contaContabilGrupo.Id.ToString()
                });
            }

            ViewBag.ListaContaContabilGrupo = listItems;

            return View(contaContabilSubGrupo);
        }

        #endregion

        #region ContaContabil

        public ActionResult ExcluirContaContabil(int? id)
        {
            if (id.HasValue)
                FuncoesContaContabil.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarContaContabil(int? id)
        {
            ContaContabil contaContabil = (id.HasValue) ? FuncoesContaContabil.Load((int) id) : new ContaContabil();

            List<ContaContabilSubGrupo> listContaContabilSubGrupo = FuncoesContaContabilSubGrupo.Load();

            var listItems = new List<SelectListItem>();

            foreach (var contaContabilSubGrupo in listContaContabilSubGrupo)
            {
                var item = new SelectListItem
                {
                    Text = contaContabilSubGrupo.NomeNormalizado,
                    Value = contaContabilSubGrupo.Id.ToString()
                };

                if (id.HasValue && contaContabil.ContaContabilSubGrupo != null && contaContabil.ContaContabilSubGrupo.Id == contaContabilSubGrupo.Id)
                {
                    item.Selected = true;
                }

                listItems.Add(item);
            }
            ViewBag.ListaContaContabilSubGrupo = listItems;
            ViewBag.ListaGrupoDRE = ListGrupoDre();
            var listExisteLucroPrejuizo = FuncoesContaContabil.LoadWithSeekByHqlObject().Where(x => x.LucroPrejuizoAcumulado == EnumSimNao.Sim);
            if (listExisteLucroPrejuizo.Count() >= 1)
            {
                ViewBag.ExisteLucroPrejuizoAcumulado = 1;
            }
            else
            {
                ViewBag.ExisteLucroPrejuizoAcumulado = 0;
            }

                return PartialView("CadastrarEditarContaContabil", contaContabil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarContaContabil(ContaContabil contaContabil)
        {
            ModelState["ContaContabilSubGrupo.Id"].Errors.Clear();
            ModelState["ContaContabilSubGrupo.Codigo"].Errors.Clear();
            ModelState["ContaContabilSubGrupo.ContaContabilGrupo"].Errors.Clear();
            ModelState["Id"].Errors.Clear();
            ModelState["CampoHelp.Nome"].Errors.Clear();
            ModelState["GrupoDRE.Id"].Errors.Clear();

            if (contaContabil.LucroPrejuizoAcumulado == 0)
            {
                contaContabil.LucroPrejuizoAcumulado = EnumSimNao.Nao;
            }
            if (contaContabil.ExigeSaldoinicial == 0)
            {
                contaContabil.ExigeSaldoinicial = EnumExigeSaldoinicial.Nao;
            }

            var help = FuncoesContaContabil.Load(contaContabil.Id);
            if (help != null)
            {
                help.CampoHelp = contaContabil.CampoHelp;
                help.CampoNome = contaContabil.CampoNome;
                help.Codigo = contaContabil.Codigo;
                help.ContaContabilSubGrupo = contaContabil.ContaContabilSubGrupo;
                help.ExigeSaldoinicial = contaContabil.ExigeSaldoinicial;
                help.GrupoCodigo = contaContabil.GrupoCodigo;
                help.GrupoDRE = contaContabil.GrupoDRE;
                help.GrupoNome = contaContabil.GrupoNome;
                help.LucroPrejuizoAcumulado = contaContabil.LucroPrejuizoAcumulado;
                help.NotaExplicativaContaContabil = contaContabil.NotaExplicativaContaContabil;
                help.NotaExplicativaGrupo = contaContabil.NotaExplicativaGrupo;
                help.NotaExplicativaSubGrupo = contaContabil.NotaExplicativaSubGrupo;
                help.SubGrupoCodigo = contaContabil.SubGrupoCodigo;
                help.SubGrupoNome = contaContabil.SubGrupoNome;
                help.TipoContaContabil = contaContabil.TipoContaContabil;
                help.Id = contaContabil.Id;
                if (help.GrupoDRE.Id == 0)
                {
                    help.GrupoDRE = null;
                }
            }
            if (ModelState.IsValid)
            {
                if (help != null)
                {
                    contaContabil.Help = help.Help;
                    FuncoesContaContabil.Salvar(help);
                }
                else
                {
                    if (contaContabil.GrupoDRE.Id == 0)
                    {
                        contaContabil.GrupoDRE = null;
                    }
                    FuncoesContaContabil.Salvar(contaContabil);
                }

                Thread.Sleep(500);
                return RedirectToAction("Index");
            }

            List<ContaContabilSubGrupo> listContaContabilGrupo = FuncoesContaContabilSubGrupo.Load();

            var listItems = new List<SelectListItem>();

            foreach (var contaContabilGrupo in listContaContabilGrupo)
            {
                listItems.Add(new SelectListItem
                {
                    Text = contaContabilGrupo.NomeNormalizado,
                    Value = contaContabilGrupo.Id.ToString()
                });
            }

            ViewBag.ListaContaContabilSubGrupo = listItems;

            return View(contaContabil);
        }

        private List<SelectListItem> ListGrupoDre()
        {
            List<GrupoDRE> listGrupoDre = FuncoesGrupoDre.Load();
            var listItemsGrupoDre = new List<SelectListItem>();

            foreach (var conta in listGrupoDre)
            {
                var item = new SelectListItem
                {
                    Text = conta.Nome,
                    Value = conta.Id.ToString()
                };

                listItemsGrupoDre.Add(item);
            }

            return listItemsGrupoDre;
        }

        [HttpGet]
        public ActionResult CadastrarEditarContaContabilHelp(int? id)
        {
            var contaHelp = (id.HasValue) ? FuncoesContaContabil.Load((int)id) : new ContaContabil();

            return PartialView("CadastrarEditarContaContabilHelp", contaHelp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadastrarEditarContaContabilHelp(ContaContabil contaHelp, FormCollection form)
        {
            ModelState["Codigo"].Errors.Clear();
            ModelState["CampoNome"].Errors.Clear();
            var conta = FuncoesContaContabil.Load(contaHelp.Id);
            if (ModelState.IsValid)
            {
                conta.Help = contaHelp.Help;
                FuncoesContaContabil.Salvar(conta);
                Thread.Sleep(1000);
                return RedirectToAction("Index");
            }

            return View(contaHelp);
        }
        #endregion


    }

}
