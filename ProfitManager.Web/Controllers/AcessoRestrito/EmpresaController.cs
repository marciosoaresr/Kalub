using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NHibernate;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    [HandleError]
    public class EmpresaController : BaseController
    {

        public ActionResult Index()
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();


            ViewBag.empresaDados = FuncoesEmpresaUsuario.BuscarEmpresa(empresaUsuario.Empresa.Id);
            ViewBag.empresaUsuarioDados = FuncoesEmpresaUsuario.BuscarEmpresaUsuario(empresaUsuario.Empresa.Id);
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;

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

        public ActionResult Usuarios(int? pageNumber, string search)
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            ViewBag.status = empresaUsuario.Empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            List<EmpresaUsuario> listUsuarios = FuncoesEmpresaUsuario.Carrega(empresaUsuario.Empresa.Id);

            const int pageSize = 10;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listUsuarios != null && listUsuarios.Count > 0 ? (listUsuarios.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

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

            return View(listUsuarios.ToPagedList(pageCurrent, pageSize));
        }
        public ActionResult UsuariosLogs(int? pageNumber, string search)
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            ViewBag.status = empresaUsuario.Empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            List<EmpresaUsuarioLogs> listUsuariosLogs = FuncoesEmpresaUsuarioLogs.Carrega(empresaUsuario.Empresa.Id);

            var diario = listUsuariosLogs.Where(x => x.DataHoraCriacao.Value.ToShortDateString() == DateTime.Now.ToShortDateString());
            ViewBag.totalDiario = diario.Count();
            int total = listUsuariosLogs.Count;
            ViewBag.totalAcessos = total;
            const int pageSize = 20;
            int pageCurrent = (pageNumber ?? 1);
            ViewBag.PageCount = listUsuariosLogs != null && listUsuariosLogs.Count > 0 ? (listUsuariosLogs.Count + pageSize - 1) / pageSize : 1;
            ViewBag.PageNumber = pageCurrent;
            ViewBag.Search = search;

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            return View(listUsuariosLogs.ToPagedList(pageCurrent, pageSize));
        }
        public ActionResult Empresa()
        {
            EmpresaUsuario empresa =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();

            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            ViewBag.ListaCidade = FuncoesCidade.Load();

            return View();
        }


        [HttpGet]
        public ActionResult CadastrarEditarEmpresa()
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            var empresa = FuncoesEmpresa.Load((int)empresaUsuario.Empresa.Id);
            ViewBag.ListaCidade = ListCidade(empresa);
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(empresaUsuario.Empresa).OrderBy(x => x.Text);

            ViewBag.status = empresaUsuario.Empresa.Status;
            if (ViewBag.status == EnumEmpresaStatus.Bloqueado)
            {
                return RedirectToAction("Suspenso", "EmpresaRecebimento");
            }

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;
            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.Logomarca = empresaUsuario.Empresa.Logomarca;

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

            return View("Empresa", empresa);
        }


        [HttpPost]
        public ActionResult CadastrarEditarEmpresa(Empresa empresa, FormCollection form, HttpPostedFileBase Logomarca)
        {

            ModelState["Cnpj"].Errors.Clear();
            ModelState["Logradouro"].Errors.Clear();
            ModelState["Numero"].Errors.Clear();
            ModelState["Complemento"].Errors.Clear();
            ModelState["Bairro"].Errors.Clear();
            ModelState["Cep"].Errors.Clear();

            ModelState["Cidade.Id"].Errors.Clear();
            ModelState["CategoriaEmpresa.Codigo"].Errors.Clear();
            ModelState["CategoriaEmpresa.CampoNome"].Errors.Clear();
            ModelState["Telefone1"].Errors.Clear();
            ModelState["Telefone2"].Errors.Clear();
            ModelState["Email"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (Logomarca != null)
                {
                    string pic = Path.GetFileName(Logomarca.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Logomarcas"), pic);
                    Logomarca.SaveAs(path);

                    empresa.Logomarca = pic;
                }
                FuncoesEmpresa.Salvar(empresa);
                return RedirectToAction("CadastrarEditarEmpresa");
            }

            return RedirectToAction("CadastrarEditarEmpresa");
        }

        [HttpGet]
        public ActionResult CadastrarEditarEmpresaUsuario(int? id)
        {
            var empresaDados = (id.HasValue) ? FuncoesEmpresaUsuario.BuscarEmpresaUsuario((int)id) : new EmpresaUsuario();

            return PartialView("CadastrarEditarEmpresaUsuario", empresaDados);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEditarEmpresaUsuario(EmpresaUsuario empresaUsuario, FormCollection form)
        {
            var empresaIdUsuario = FuncoesEmpresaUsuario.BuscarEmpresaUsuario((int)empresaUsuario.Id);

            if (empresaIdUsuario != null)
            {
                empresaUsuario.Id = empresaIdUsuario.Id;
                empresaUsuario.Empresa = empresaIdUsuario.Empresa;
                empresaUsuario.Cnpj = empresaIdUsuario.Cnpj;
            }
            else
            {
                empresaUsuario.Id = 0;
                empresaUsuario.Empresa = empresaIdUsuario.Empresa;
                empresaUsuario.Cnpj = empresaIdUsuario.Cnpj;
            }

            ModelState["Cnpj"].Errors.Clear();

            if (ModelState.IsValid)
            {
                empresaUsuario.Senha = RepositorioWebCriptografia.Criptografar(empresaUsuario.Senha);
                FuncoesEmpresaUsuario.Salvar(empresaUsuario);
                return RedirectToAction("Usuarios", "Empresa");
            }

            return View("Index");
        }
        public JsonResult GetCidades(string text)
        {
            var listCidades = new List<Cidade>();

            if (!string.IsNullOrEmpty(text.Trim()))
            {
                listCidades = FuncoesCidade.LoadByName(text);
            }

            return Json(listCidades, JsonRequestBehavior.AllowGet);
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

            return listItemsCategoriaEmpresa;
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
