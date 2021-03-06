using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using iugu.net.Lib;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json.Linq;
using NHibernate;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.Auxiliar;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito
{
    public class EmpresaRecebimentoController : Controller
    {
        public ActionResult Faturas(int? pageNumber, string search)
        {
            EmpresaUsuario empresaUsuario =
                FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
                new EmpresaUsuario();

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.EmpresaEmail = empresaUsuario.Empresa.Email;
            if (empresaUsuario.Empresa.Cep != null)
            {
                ViewBag.Cep = empresaUsuario.Empresa.Cep.Replace(".", "").Replace("-", "");
            }
            ViewBag.Endereco = empresaUsuario.Empresa.Logradouro;
            ViewBag.Bairro = empresaUsuario.Empresa.Bairro;
            if (empresaUsuario.Empresa.Cidade != null)
            {
                ViewBag.Cidade = empresaUsuario.Empresa.Cidade;

            }
            if (empresaUsuario.Empresa.Cidade != null)
            {
                ViewBag.Estado = empresaUsuario.Empresa.Cidade.Estado;
            }
            ViewBag.Telefone = empresaUsuario.Empresa.Telefone1.Substring(4).Replace(" ", "");

            int dias = Convert.ToInt32(FuncoesEmpresa.CalculaPeriodoGratis(empresaUsuario.Empresa.Id));
            ViewBag.diasVencimento = dias;
            ViewBag.status = empresaUsuario.Empresa.Status;

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;

            List<EmpresaTransacaoIugu> listFaturas = FuncoesEmpresaTransacaoIugu.LoadFaturas(empresaUsuario.Empresa.Id);

            ViewBag.Faturas = listFaturas;
            ViewBag.qtdFaturas = listFaturas.Count;
            ViewBag.totalFaturas = listFaturas.Sum(x => x.Valor);

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

        public ActionResult Retorno()
        {

            return View();
        }

        public ActionResult Suspenso()
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


            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            return View();
        }
        public ActionResult PeriodoEncerrado()
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();
            ViewBag.status = empresaUsuario.Empresa.Status;
            bool acessoMobile = false;
            if (VerificaMobile())
            {
                acessoMobile = true;
            }
            ViewBag.Mobile = acessoMobile;

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;

            string msgDiasUso = FuncoesEmpresa.MensagemDiasUso(empresaUsuario.Empresa.Id);
            ViewBag.diasUso = msgDiasUso;

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            return View();
        }
        public ActionResult Contrate()
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
            ViewBag.status = empresaUsuario.Empresa.Status;

            string msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            return View();
        }

        [HttpPost]
        public ActionResult Retorno(FormCollection colecao)
        {
            string connectionString = "Data Source=kalub.database.windows.net;Database=Kalub;User ID=idkalub;Password=k11072016$kalub";
            //string connectionString = "Data Source=192.168.16.13;Database=ProfitManager2;User ID=es;Password=a01espassdes#";

            string evento = colecao[0].ToString();
            string id = null;
            string assunto = null;
            string statusFatura = null;
            string idAssinatura = null;

            if (evento == "subscription.created")
            {
                id = colecao[1].ToString();
                assunto = "Assinatura Criada: " + id;
            }
            if (evento == "invoice.created")
            {
                id = colecao[1].ToString();
                idAssinatura = colecao[4].ToString();
                statusFatura = colecao[2].ToString();
                assunto = "Fatura Criada: " + id + " colecao 0:" + colecao[0].ToString() + " colecao 2:" + colecao[2].ToString() + " colecao 3:" + colecao[3].ToString() + " colecao 4:" + colecao[4].ToString();
                string idEmpresa = null;

                SqlDataReader rdr = null;
                SqlConnection conn = new SqlConnection(connectionString);
                var strSql =
                    "SELECT * " +
                    "FROM EmpresaAssinaturaIugu " +
                    "Where IdAssinatura = '" + idAssinatura + "'";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    idEmpresa = (string)rdr["Empresa"];
                }
                conn.Close();

                if (idEmpresa != "")
                {
                    bool existeFatura = false;
                    SqlDataReader rdr1 = null;
                    SqlConnection conn1 = new SqlConnection(connectionString);
                    var strSql1 =
                        "SELECT * " +
                        "FROM EmpresaTransacaoIugu " +
                        "Where IdAssinatura = '" + idAssinatura + "'";
                    SqlCommand cmd1 = new SqlCommand(strSql1, conn1);
                    conn1.Open();
                    rdr1 = cmd1.ExecuteReader();
                    while (rdr1.Read())
                    {
                        existeFatura = true;
                    }
                    conn1.Close();

                    if (!existeFatura)
                    {
                        SqlConnection conn2 = new SqlConnection(connectionString);
                        var transaction = conn2.BeginTransaction(IsolationLevel.ReadUncommitted);

                        var strSqlGeraCobranca =
                            "Insert Into EmpresaTransacaoIugu (DataHoraCriacao, IdAssinatura, IdFatura, Status, Empresa) " +
                            "(Select GetDate(), '" + idAssinatura + "', '" + id + "', '" + statusFatura + "' , '" + idEmpresa + "')";

                        SqlCommand c = new SqlCommand(strSqlGeraCobranca, conn2, transaction);

                        c.CommandTimeout = 600;
                        DataTable dt = new DataTable();
                        var reader = c.ExecuteReader();
                        dt.Load(reader);
                        transaction.Commit();
                        reader.Close();
                        conn2.Close();

                    }
                }
            }
            if (evento == "invoice.status_changed")
            {

                id = colecao[1].ToString();
                statusFatura = colecao[2].ToString();
                idAssinatura = colecao[4].ToString();

                //AtualizaStatusFatura(id);

                assunto = "Id Fatura status_changed: " + id + " Status Fatura:" + statusFatura;

                SqlConnection conexao = new SqlConnection(connectionString);
                conexao.Open();
                var transaction = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);

                var strSql =
                    "UPDATE EmpresaTransacaoIugu " +
                    "SET Status = '" + statusFatura + "'" +
                    "WHERE IdFatura = '" + id + "'";

                SqlCommand c = new SqlCommand(strSql, conexao, transaction);
                c.CommandTimeout = 600;
                transaction.Commit();
                DataTable dt = new DataTable();
                var reader = c.ExecuteReader();
                dt.Load(reader);
                reader.Close();

                if (statusFatura != "canceled")
                {
                    var transactionEmpresa = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);
                    var strSqlEmpresa =
                        "UPDATE Empresa " +
                        "SET Status = 'A'" +
                        "WHERE IdAssinatura = '" + idAssinatura + "'";
                    SqlCommand cc = new SqlCommand(strSqlEmpresa, conexao, transactionEmpresa);
                    cc.CommandTimeout = 600;
                    transactionEmpresa.Commit();
                    DataTable dtc = new DataTable();
                    var readerc = cc.ExecuteReader();
                    dtc.Load(readerc);
                    readerc.Close();
                }


                conexao.Close();

            }

            string sUserName = "no-reply@kalub.com.br";
            string sPassword = "k28062016";

            MailMessage objEmail = new MailMessage();
            objEmail.To.Add("marcio@esolution.com.br");
            objEmail.From = new MailAddress("no-reply@kalub.com.br");
            objEmail.Subject = evento;
            objEmail.Body = assunto;
            objEmail.Priority = MailPriority.Normal;
            objEmail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.kalub.com.br";
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(sUserName, sPassword);
            smtp.EnableSsl = false;
            smtp.Send(objEmail);

            return View();
        }

        //async static void AtualizaStatusFatura(string idFatura)
        //{
        //    using (Invoice apiInvoice = new Invoice())
        //    {
        //        var invoices = await apiInvoice.GetAsync(idFatura).ConfigureAwait(false);
        //        var statusiugu = invoices.status;
        //        //var dataVencimento = invoices.due_date;
        //        var email = invoices.email;
        //        //var secureId = invoices.secure_id;
        //        //var secureUrl = invoices.secure_url;
        //        //var valor = invoices.total;

        //        //var total = valor.Replace("R$ ", "");
        //        //total = total.Replace(",", ".");
        //        //var data = dataVencimento.ToString("yyyy-MM-dd");

        //        var empresa = FuncoesEmpresaUsuario.BuscarEmpresaEmail(email);
        //        var empresaTransacaoIugu = new EmpresaTransacaoIugu();
        //        //empresaTransacaoIugu.DataVencimento = Convert.ToDateTime(dataVencimento);
        //        //empresaTransacaoIugu.Email = Convert.ToString(email);
        //        empresaTransacaoIugu.Status = Convert.ToString(statusiugu);
        //        //empresaTransacaoIugu.Valor = Convert.ToDecimal(valor);
        //        //empresaTransacaoIugu.SecureId = Convert.ToString(secureId);
        //        //empresaTransacaoIugu.SecureUrl = Convert.ToString(secureUrl);
        //        empresaTransacaoIugu.Empresa = empresa;
        //        FuncoesEmpresaTransacaoIugu.Salvar(empresaTransacaoIugu);
        //    }
        //}

        public ActionResult RetornoAprovado()
        {
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
