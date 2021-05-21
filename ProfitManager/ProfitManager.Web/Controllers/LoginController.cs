using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NHibernate.Linq.Functions;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Fabrica.Excecao;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers
{

    public class LoginController : Controller
    {

        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";


        public ActionResult Login()
        {
            var usuario = RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true);
            if (usuario != null)
            {
                if (usuario.Administrador == EnumSimNao.Sim)
                    return RedirectToAction("Index", "Principal");

                return RedirectToAction("Index", "PrincipalUser");
            }

            return View();
        }

        public ActionResult Registrar(string erro)
        {
            var usuario = RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true);

            if (erro != null)
            {
                ViewBag.emailExistente = erro;
            }
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(null).OrderBy(x => x.Text);
            ViewBag.termo = FuncoesTermoUso.Load(1);
            ViewBag.tipo = Request.QueryString["TipoCadastro"] == "P" ? EnumTipoCadastro.Professor : EnumTipoCadastro.Empresa;
            ViewBag.TipoCadastro = Request.QueryString["TipoCadastro"];

            return View();
        }


        public ActionResult LoginAdm()
        {
            var usuario = RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true);
            if (usuario != null)
            {
                if (usuario.Administrador == EnumSimNao.Sim)
                    return RedirectToAction("Index", "Principal");

                return RedirectToAction("Index", "PrincipalUser");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AutenticarAdm(FormCollection form)
        {
            string email = form["email"];
            string senha = form["senha"];

            if (!RepositorioWebUsuario.AutenticarUsuario(email, RepositorioWebCriptografia.Criptografar(senha),
                    EnumSimNao.Sim))
            {
                ViewBag.Notification = new NotificationException("O e-mail ou a senha inseridos estão incorretos",
                    EnumTypeException.Information);

                return View("LoginAdm");
            }

            return RedirectToAction("Index", "Principal");
        }

        [HttpPost]
        public ActionResult Autenticar(FormCollection form)
        {
            string email = form["email"];
            string senha = form["senha"];

            if (email == "kalub.contabil")
            {
                if (!RepositorioWebUsuario.AutenticarUsuario(email, RepositorioWebCriptografia.Criptografar(senha),
                EnumSimNao.Sim))
                {
                    ViewBag.Notification = new NotificationException("O login ou a senha inseridos estão incorretos",
                        EnumTypeException.Information);

                    return View("Login");
                }
                else
                {
                    return View("LoginCnpj");
                }

            }
            else
            {
                if (!RepositorioWebUsuario.AutenticarUsuario(email, RepositorioWebCriptografia.Criptografar(senha),
                    EnumSimNao.Nao))
                {
                    ViewBag.Notification = new NotificationException("O e-mail ou a senha inseridos estão incorretos",
                        EnumTypeException.Information);

                    return View("Login");
                }

            }
            ViewBag.usuarioLogado = System.Web.HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            return RedirectToAction("Index", "Principal");
        }

        [HttpPost]
        public ActionResult AutenticarCnpj(FormCollection form)
        {
            var usuario = RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true);
            string cnpj = form["cnpj"];

            if (!RepositorioWebUsuario.AutenticarCnpj(cnpj, EnumSimNao.Nao, usuario.Email))
            {
                ViewBag.Notification = new NotificationException("O Cnj esta incorreto", EnumTypeException.Information);
                return View("LoginCnpj");
            }

            return RedirectToAction("Index", "Principal");
        }

        public ActionResult LogoutAdm()
        {
            RepositorioWebUsuario.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout(FormCollection form)
        {
            RepositorioWebUsuario.Logout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogoutMobile()
        {
            RepositorioWebUsuario.Logout();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {

            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id);

            var dataLancamento = DateTime.Now;
            dataLancamento = dataLancamento.AddDays(-1);

            ViewBag.status = empresaUsuario.Empresa.Status;

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;

            if (empresaUsuario != null)
            {


                var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);

                //ViewBag.ListaDRE = FuncoesGrupoDre.CarregaDre(empresaUsuario.Empresa.Id, DateTime.Today);

                ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;
                ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
                ViewBag.EmpresaEmail = empresaUsuario.Empresa.Email;
                ViewBag.EmpresaId = empresaUsuario.Empresa.Id;

            }

            // Verifica empresa tipo Professor tem exercicio criado para liberar o acesso aos
            // saldos iniciais e lancamentos 
            EnumTipoCadastro tipo = empresaUsuario.Empresa.TipoCadastro;
            List<ExercicioEmpresa> listExercicioEmpresa = FuncoesExercicioEmpresa.Load();
            listExercicioEmpresa = listExercicioEmpresa.Where(x => x.Empresa.Id == empresaUsuario.Empresa.Id).OrderBy(x => x.Id).ToList();
            bool acessoExercicio = listExercicioEmpresa.Count > 0;
            ViewBag.existeExercicio = false;
            if (tipo == EnumTipoCadastro.Professor && acessoExercicio)
            {
                ViewBag.existeExercicio = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Empresa empresa, FormCollection form)
        {
            string email = form["Email"];
            string senha = form["senha1"];
            string tipoCadastro = form["TipoCadastro"];
            string professorAluno = form["professorAluno"];
            int categoriaEmpresa = Convert.ToInt32(form["CategoriaEmpresa.Id"]);

            if (categoriaEmpresa != 0)
            {
                ModelState["CategoriaEmpresa.Codigo"].Errors.Clear();
                ModelState["CategoriaEmpresa.CampoNome"].Errors.Clear();
            }
            ModelState["NomeFantasia"].Errors.Clear();
            ModelState["Cnpj"].Errors.Clear();
            ModelState["Logradouro"].Errors.Clear();
            ModelState["Numero"].Errors.Clear();
            ModelState["Complemento"].Errors.Clear();
            ModelState["Bairro"].Errors.Clear();
            ModelState["Cep"].Errors.Clear();
            ModelState["Telefone2"].Errors.Clear();
            ModelState["Cidade"].Errors.Clear();
            ModelState["CupomPromocional"].Errors.Clear();
            ModelState["TipoCadastro"].Errors.Clear();
            //ModelState["professorAluno"].Errors.Clear();

            empresa.PeriodoGratis = "7";
            empresa.PeriodoUsoVencido = "10";
            empresa.Status = EnumEmpresaStatus.Ativo; //19/06/2017 - EnumEmpresaStatus.Gratis
            int diaVencimento = DateTime.Today.Day;
            empresa.DiaVencimento = diaVencimento;

            // Professores e alunos
            if (categoriaEmpresa == 27)
            {
                empresa.PeriodoGratis = null;
                empresa.PeriodoUsoVencido = "10";
                empresa.Status = EnumEmpresaStatus.Ativo;
            }

            //if (tipoPlano == 2) // Aluno
            //{
            //    empresa.PeriodoGratis = "30";
            //}
            //if (tipoPlano == 3) // Professor
            //{
            //    empresa.PeriodoGratis = "30";
            //}
            //if (tipoPlano == 4) // Consultor
            //{
            //    empresa.PeriodoGratis = "30";
            //}

            empresa.DataAbertura = DateTime.Now;
            empresa.NomeFantasia = empresa.Nome;

            empresa.TipoCadastro = tipoCadastro == "P" ? EnumTipoCadastro.Professor : EnumTipoCadastro.Empresa;
            empresa.TipoProfessorAluno = professorAluno == "P" ? EnumTipoProfessorAluno.Professor : EnumTipoProfessorAluno.Aluno;

            if (categoriaEmpresa == 0)
            {
                empresa.CategoriaEmpresa = null;
            }

            if (ValidaEmail(email))
            {
                List<Empresa> emailExistenteEmpresa = FuncoesEmpresa.EmailExistente(email);
                if (emailExistenteEmpresa.Count > 0)
                {
                    string erro = "E-MAIL informado ja existe em nosso sistema!";
                    return RedirectToAction("Registrar", new { erro = erro });
                }
                List<EmpresaUsuario> emailExistenteEmpresaUsuario = FuncoesEmpresaUsuario.EmailExistente(email);
                if (emailExistenteEmpresaUsuario.Count > 0)
                {
                    string erro = "E-MAIL informado ja existe em nosso sistema!";
                    return RedirectToAction("Registrar", new { erro = erro });
                }
            }
            else
            {
                string erro = "Por favor, informe um E-MAIL válido!";
                return RedirectToAction("Registrar", new { erro = erro });
            }



            if (ModelState.IsValid)
            {
                FuncoesEmpresa.Salvar(empresa);
                var empresaUsuario = new EmpresaUsuario();
                empresaUsuario.Empresa = empresa;
                empresaUsuario.Nome = empresa.NomeFantasia;
                empresaUsuario.Email = email;
                empresaUsuario.Login = email;
                empresaUsuario.Cnpj = "00.000.000/0001-00";
                empresaUsuario.Senha = RepositorioWebCriptografia.Criptografar(senha);
                FuncoesEmpresaUsuario.Salvar(empresaUsuario);
                FuncoesEmail.EnviarEmail(email, EnumTemplateEmail.EmailBoasVindas, senha);
                RepositorioWebUsuario.AutenticarUsuario(email, RepositorioWebCriptografia.Criptografar(senha),
                    EnumSimNao.Nao);
            }
            return RedirectToAction("Index", "PrincipalUser");
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

        public static bool ValidaEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        public ActionResult RecuperarSenha(string msg, string erro)
        {
            if (msg != null)
            {
                ViewBag.msg = msg;
            }
            if (erro != null)
            {
                ViewBag.erro = erro;
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(FormCollection form)
        {
            string email = form["Email"];
            string msgerro = null;
            string msgok = null;
            var empresaUsuario = FuncoesEmpresaUsuario.BuscarEmpresaUsuarioEmail(email);
            if (empresaUsuario != null)
            {
                string senha = RepositorioWebCriptografia.Descriptografar(empresaUsuario.Senha);
                FuncoesEmail.EnviarEmail(email, EnumTemplateEmail.EmailRecuperaSenha, senha);
                msgok = "As instruções foram enviadas por e-mail!";
            }
            else
            {
                msgerro = "E-mail informado não foi localizado!";
            }
            return RedirectToAction("RecuperarSenha", "Login", new { msg = msgok, erro = msgerro });
        }

    }
}
