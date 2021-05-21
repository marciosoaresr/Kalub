using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NHibernate;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class CadastroEmpresaController : Controller
    {

        public ActionResult Index(int? pageNumber, string search)
        {
            List<Empresa> listEmpresa = (search.HasValue()) ? FuncoesEmpresa.Load(search) : FuncoesEmpresa.Load();

            const int pageSize = 150;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listEmpresa != null && listEmpresa.Count > 0 ? (listEmpresa.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            return View(listEmpresa.ToPagedList(pageCurrent, pageSize));
        }

        [HttpGet]
        public ActionResult Excluir(int? id)
        {
            if (id.HasValue)
                FuncoesEmpresa.Excluir((int)id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CadastrarEditarCadastroEmpresa(int? id)
        {
            var empresa = (id.HasValue) ? FuncoesEmpresa.Load((int)id) : new Empresa();

            ViewBag.ListaCidade = ListCidade(empresa);
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(empresa);

            return PartialView("CadastrarEditarCadastroEmpresa", empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarCadastroEmpresa(Empresa empresa, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();
            ModelState["CategoriaEmpresa.Id"].Errors.Clear();
            ModelState["CategoriaEmpresa.Codigo"].Errors.Clear();
            ModelState["CategoriaEmpresa.CampoNome"].Errors.Clear();
            ModelState["Cidade.Id"].Errors.Clear();
            ModelState["Telefone2"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesEmpresa.Salvar(empresa);
                Thread.Sleep(500);
                return RedirectToAction("Index");
            }


            return View(empresa);
        }


        [HttpGet]
        public ActionResult CadastrarEditarEmpresaUsuario(int? id)
        {
            var empresaDados = (id.HasValue) ? FuncoesEmpresaUsuario.BuscarEmpresaUsuarioAdm((int)id) : new EmpresaUsuario();
            ViewBag.Senha = RepositorioWebCriptografia.Descriptografar(empresaDados.Senha);

            return PartialView("CadastrarEditarEmpresaUsuario", empresaDados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarEmpresaUsuario(EmpresaUsuario empresaUsuario, FormCollection form)
        {
            string user = form["user"];
            var empresaDados = FuncoesEmpresaUsuario.BuscarEmpresa((int)empresaUsuario.Id);
            var empresaIdUsuario = FuncoesEmpresaUsuario.BuscarEmpresaUsuario((int)empresaUsuario.Id);

            if (empresaIdUsuario != null)
            {
                empresaUsuario.Id = empresaIdUsuario.Id;
                empresaUsuario.Empresa = empresaDados;
                empresaUsuario.Cnpj = empresaDados.Cnpj;
            }
            else
            {
                empresaUsuario.Id = 0;
                empresaUsuario.Empresa = empresaDados;
                empresaUsuario.Cnpj = empresaDados.Cnpj;
            }

            ModelState["Cnpj"].Errors.Clear();

            if (ModelState.IsValid)
            {
                empresaUsuario.Senha = RepositorioWebCriptografia.Criptografar(empresaUsuario.Senha);
                FuncoesEmpresaUsuario.Salvar(empresaUsuario);
                return RedirectToAction("Index", "CadastroEmpresa");
            }

            return View("Index");
        }


        private List<SelectListItem> ListCidade(Empresa empresa)
        {
            List<Cidade> listCidade = FuncoesCidade.Load();
            var listItemsCidade = new List<SelectListItem>();

            foreach (var cidade in listCidade)
            {
                var item = new SelectListItem
                {
                    Text = cidade.NomeNormalizado,
                    Value = cidade.Id.ToString()
                };

                if (empresa.Cidade != null && cidade.Id == empresa.Cidade.Id)
                {
                    item.Selected = true;
                }

                listItemsCidade.Add(item);
            }

            return listItemsCidade;
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

                if (empresa.CategoriaEmpresa != null && categoriaempresa.Id == empresa.CategoriaEmpresa.Id)
                {
                    item.Selected = true;
                }

                listItemsCategoriaEmpresa.Add(item);
            }

            return listItemsCategoriaEmpresa.ToList();
        }

        [HttpGet]
        public ActionResult EmpresaRecalcularSaldo(int? id)
        {
            var empresa = (id.HasValue) ? FuncoesEmpresa.Load((int)id) : new Empresa();

            return PartialView("EmpresaRecalcularSaldo", empresa);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmpresaRecalcularSaldo(Empresa empresa, FormCollection form)
        {

            FuncoesEventoLancamento.RecalcularSaldoEmpresa(empresa.Id);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult EmpresaDeletar(int? id)
        {
            var empresa = (id.HasValue) ? FuncoesEmpresa.Load((int)id) : new Empresa();
            ViewBag.Empresa = empresa.Nome;
            return PartialView("EmpresaDeletar", empresa);
        }

        [HttpPost]
        public ActionResult EmpresaDeletar(Empresa empresa, FormCollection form)
        {
            FuncoesEmpresa.DeletarContaContabilSaldo(empresa.Id);
            FuncoesEmpresa.DeletarContaContabilSaldoInicial(empresa.Id);
            FuncoesEmpresa.DeletarEventoLancamento(empresa.Id);
            FuncoesEmpresa.DeletarFechamentoCaixa(empresa.Id);
            FuncoesEmpresa.DeletarEmpresaRecebimento(empresa.Id);
            FuncoesEmpresa.DeletarEmpresaRecebimentoTransacao(empresa.Id);
            FuncoesEmpresa.DeletarEmpresaUsuarioLog(empresa.Id);
            FuncoesEmpresa.DeletarEmpresaUsuario(empresa.Id);
            FuncoesEmpresa.DeletarEmpresa(empresa.Id);
            return RedirectToAction("Index");

        }
    }
}
