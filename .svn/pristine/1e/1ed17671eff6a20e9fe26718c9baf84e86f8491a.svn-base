using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using iugu.net.Entity;
using iugu.net.Lib;
using iugu.net.Request;
using iugu.net.Response;
using Newtonsoft.Json.Linq;

namespace ProfitManager.KalubServico
{
    public static class Program
    {
        //static SqlConnection _con = null;
        private const string ConnectionString = "Data Source=kalub.database.windows.net;Database=Kalub;User ID=idkalub;Password=k11072016$kalub";

        //"Data Source=192.168.16.13;Database=ProfitManager2;User ID=es;Password=a01espassdes#";
        static void Main(string[] args)
        {

            var t = AtualizaNovasFaturas(args);
            t.Wait();

            GeraBloqueio();

            EnviarEmailExperienciaEncerrado();

        }

        static async Task AtualizaNovasFaturas(string[] args)
        {
            SqlDataReader rdr = null;
            var conn = new SqlConnection(ConnectionString);
            const string strSql = "SELECT * " +
                                  "FROM EmpresaTransacaoIugu " +
                                  "Where SecureUrl is NULL ";
            var cmd = new SqlCommand(strSql, conn);
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var idFatura = (string)rdr["IdFatura"];
                using (var apiInvoice = new Invoice())
                {
                    var invoices = await apiInvoice.GetAsync(idFatura).ConfigureAwait(false);
                    var statusiugu = invoices.status;
                    var dataVencimento = invoices.due_date;
                    var email = invoices.email;
                    var secureUrl = invoices.secure_url;
                    var secureId = invoices.secure_id;
                    var valor = invoices.total;
                    AtualizaFatura(idFatura, statusiugu,Convert.ToDateTime(dataVencimento),email,secureUrl,valor,secureId);
                }

            }
            conn.Close();
        }

        static void AtualizaFatura(string idFatura, string status, DateTime dataVencimento, string email, string secureUrl, string valor, string secureId)
        {

            var conexao = new SqlConnection(ConnectionString);
            conexao.Open();
            var transaction = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);
            var total = valor.Replace("R$ ", "");
            total = total.Replace(",", ".");
            var data = dataVencimento.ToString("yyyy-MM-dd");
            var strSqlGeraBloqueio =
                "UPDATE EmpresaTransacaoIugu " +
                "SET Status = '" + status + "'," +
                "DataVencimento = '" + data + "'," +
                "Email = '" + email + "'," +
                "SecureId = '" + secureId + "'," +
                "SecureUrl = '" + secureUrl + "'," +
                "Valor = '" + total + "' " +
                "WHERE IdFatura = '" + idFatura + "'";

            var c = new SqlCommand(strSqlGeraBloqueio, conexao, transaction) {CommandTimeout = 600};
            transaction.Commit();
            var dt = new DataTable();
            var reader = c.ExecuteReader();
            dt.Load(reader);
            reader.Close();
            conexao.Close();
        }


        //static async Task BuscaStatusFaturasPending(string[] args)
        //{
        //    SqlDataReader rdr = null;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    var strSql =
        //        "SELECT et.* " +
        //        "FROM EmpresaTransacaoIugu et " +
        //        "Where et.Status = 'pending' ";
        //    SqlCommand cmd = new SqlCommand(strSql, conn);
        //    conn.Open();
        //    rdr = cmd.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        string idFatura = (string)rdr["IdFatura"];
        //        string idEmpresa = (string) rdr["Empresa"];
        //        var statusiugu = "";
        //        using (var apiInvoice = new Invoice())
        //        {
        //            var invoices = await apiInvoice.GetAsync(idFatura).ConfigureAwait(false);
        //            statusiugu = invoices.status;
        //        }
        //        AtualizaStatusFaturas(idFatura,statusiugu);
        //        if (statusiugu == "paid") //pago
        //        {
        //            AtualizaStatusEmpresa(idEmpresa);
        //        }

        //    }
        //    conn.Close();
        //}

        //static void AtualizaStatusFaturas(string idFatura, string status)
        //{

        //        SqlConnection conexao = new SqlConnection(connectionString);
        //        conexao.Open();
        //        var transaction = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);

        //        var strSqlGeraBloqueio =
        //            "UPDATE EmpresaTransacaoIugu " +
        //            "SET Status = '" + status + "'" +
        //            "WHERE IdFatura = '" + idFatura + "'";

        //        SqlCommand c = new SqlCommand(strSqlGeraBloqueio, conexao, transaction);
        //        c.CommandTimeout = 600;
        //        transaction.Commit();
        //        DataTable dt = new DataTable();
        //        var reader = c.ExecuteReader();
        //        dt.Load(reader);
        //        reader.Close();
        //        conexao.Close();
        //}

        //static void AtualizaStatusEmpresa(string idEmpresa)
        //{

        //    SqlConnection conexao = new SqlConnection(connectionString);
        //    conexao.Open();
        //    var transaction = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);

        //    var strSqlGeraBloqueio =
        //        "UPDATE Empresa " +
        //        "SET Status = 'A' " +
        //        "FROM Empresa " +
        //        "WHERE Id = '" + idEmpresa + "'";

        //    SqlCommand c = new SqlCommand(strSqlGeraBloqueio, conexao, transaction);
        //    c.CommandTimeout = 600;
        //    transaction.Commit();
        //    DataTable dt = new DataTable();
        //    var reader = c.ExecuteReader();
        //    dt.Load(reader);
        //    reader.Close();
        //    conexao.Close();

        //}

        static void GeraBloqueio()
        {

            SqlConnection conexao = new SqlConnection(ConnectionString);
            conexao.Open();
            var transaction = conexao.BeginTransaction(IsolationLevel.ReadUncommitted);
            DateTime vencimento = DateTime.Today;
            string strVencimento = $"{vencimento.AddDays(-5):yyyy-MM-dd}";

            var strSqlGeraBloqueio =
                "UPDATE Empresa " +
                "SET Status = 'B' " +
                "FROM Empresa e INNER JOIN EmpresaTransacaoIugu et " +
                "ON e.Id = et.Empresa " +
                "WHERE et.DataVencimento <= '" + strVencimento + "'" +
                "AND et.Status = 'pending' ";

            SqlCommand c = new SqlCommand(strSqlGeraBloqueio, conexao, transaction) {CommandTimeout = 600};
            transaction.Commit();
            DataTable dt = new DataTable();
            var reader = c.ExecuteReader();
            dt.Load(reader);
            reader.Close();
            conexao.Close();
        }

        static void EnviarEmailExperienciaEncerrado()
        {

            SqlDataReader rdr = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            var strSql =
                "SELECT e.* " +
                "FROM Empresa e " +
                "Where e.Status = 'G' ";

            SqlCommand cmd = new SqlCommand(strSql, conn);
            try
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    DateTime dataEmpresa = (DateTime)rdr["DataHoraCriacao"];
                    string periodoGratis = (string)rdr["PeriodoGratis"];

                    DateTime dataCadastro = dataEmpresa;
                    DateTime dataAtual = DateTime.Now;

                    TimeSpan ts = dataAtual - dataCadastro;
                    int differenceInDays = (Convert.ToInt32(periodoGratis) - ts.Days);

                    string dias = Convert.ToString(differenceInDays);
                    int dia = Convert.ToInt32(dias);
                    if (dia == 1)
                    {
                        string empresa = (string)rdr["NomeFantasia"];
                        string email = (string)rdr["Email"];
                        string assunto = "Seus dias de experiência no KALUB chegaram ao fim";
                        string template = "/EmailExperienciaEncerrado.html";
                        EnviandoEmail(empresa, email, null, null, assunto, template);

                    }

                }
            }
            finally
            {
                rdr?.Close();
                conn.Close();
            }

        }

        static void EnviandoEmail(string empresa, string email, string vencimento, string gerado, string assunto, string template)
        {
            WebClient wc = new WebClient {Encoding = Encoding.UTF8};

            string assuntoEmail = assunto;
            string templateEmail = template;
            string sTemplate = wc.DownloadString("http://www.kalub.com.br" + templateEmail);

            string sEmpresa = empresa;
            sTemplate = sTemplate.Replace("##Empresa##", sEmpresa);
            string sVencimento = vencimento;
            sTemplate = sTemplate.Replace("##Vencimento##", sVencimento);
            string sGerado = gerado;
            sTemplate = sTemplate.Replace("##Gerado##", sGerado);

            string sUserName = "no-reply@kalub.com.br";
            string sPassword = "k28062016";

            MailMessage objEmail = new MailMessage();
            objEmail.To.Add(email);
            objEmail.From = new MailAddress("no-reply@kalub.com.br");
            objEmail.Subject = assuntoEmail;
            objEmail.Body = sTemplate;
            objEmail.Priority = MailPriority.Normal;
            objEmail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.kalub.com.br";
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(sUserName, sPassword);
            smtp.EnableSsl = false;
            smtp.Send(objEmail);
        }

        private class Processo
        {
            //public void GeraCobranca()
            //{

            //    var transaction = _con.BeginTransaction(IsolationLevel.ReadUncommitted);
            //    DateTime vencimento = DateTime.Now.Date.AddDays(10);
            //    string strVencimento = String.Format("{0:MM/dd/yyyy}", vencimento);

            //    var strSqlGeraCobranca =
            //        "Insert Into EmpresaRecebimento (DataHoraCriacao, Empresa, Valor, DataVencimento, Status, EmailTransacao) " +
            //        "(Select GetDate(), e.Id, 39.9, '" + strVencimento + "' , 'T', e.Email From Empresa e Where e.Status = 'A' and e.DiaVencimento = " + vencimento.Day + ")";
                
            //    SqlCommand c = new SqlCommand(strSqlGeraCobranca, _con, transaction);

            //    c.CommandTimeout = 600;
            //    DataTable dt = new DataTable();
            //    var reader = c.ExecuteReader();
            //    dt.Load(reader);
            //    transaction.Commit();
            //    reader.Close();
                
            //}

            //public void EnviarEmail()
            //{

            //    SqlDataReader rdr = null;
            //    SqlConnection conn = new SqlConnection(connectionString);
            //    DateTime vencimento = DateTime.Now.Date.AddDays(10);
            //    DateTime gerado = DateTime.Now.Date;
            //    string strVencimento = String.Format("{0:dd/MM/yyyy}", vencimento);
            //    string strGerado = String.Format("{0:dd/MM/yyyy}", gerado);
            //    var strSql =
            //        "SELECT e.NomeFantasia, e.Email " +
            //        "FROM Empresa e INNER JOIN EmpresaRecebimento er " +
            //        "ON e.Id = er.Empresa " +
            //        "Where e.Status = 'A' and er.Status = 'T' and e.DiaVencimento = " + vencimento.Day;

            //    SqlCommand cmd = new SqlCommand(strSql, conn);
            //    try
            //    {
            //        conn.Open();
            //        rdr = cmd.ExecuteReader();
            //        while (rdr.Read())
            //        {
            //            string empresa = (string)rdr["NomeFantasia"];
            //            string email = (string)rdr["Email"];
            //            string assunto = "KALUB - Lembrete de Fatura";
            //            string template = "/EmailLembreteFatura.html";
            //            EnviandoEmail(empresa, email, strVencimento, strGerado, assunto, template);
            //        }
            //    }
            //    finally
            //    {
            //        if (rdr != null)
            //        {
            //            rdr.Close();
            //        }
            //        if (conn != null)
            //        {
            //            conn.Close();
            //        }
            //    }

            //}

            //public void EnviarEmailFaturaVencendo()
            //{

            //    SqlDataReader rdr = null;
            //    SqlConnection conn = new SqlConnection(connectionString);
            //    DateTime gerado = DateTime.Today.Date;
            //    string strGerado = String.Format("{0:yyyy-MM-dd}", gerado);
            //    string strVencimento = String.Format("{0:dd/MM/yyyy}", gerado);
            //    var strSql =
            //        "SELECT e.NomeFantasia, er.EmailTransacao " +
            //        "FROM Empresa e INNER JOIN EmpresaRecebimento er " +
            //        "ON e.Id = er.Empresa " +
            //        "WHERE er.DataVencimento = '" + strGerado + "'" +
            //        "AND er.Status = 'T' ";

            //    SqlCommand cmd = new SqlCommand(strSql, conn);
            //    try
            //    {
            //        conn.Open();
            //        rdr = cmd.ExecuteReader();
            //        while (rdr.Read())
            //        {
            //            string empresa = (string)rdr["NomeFantasia"];
            //            string email = (string)rdr["EmailTransacao"];
            //            string assunto = "KALUB - Lembrete de Fatura Vencendo";
            //            string template = "/EmailLembreteFaturaVencida.html";
            //            EnviandoEmail(empresa, email, strVencimento, strGerado, assunto,template);
            //        }
            //    }
            //    finally
            //    {
            //        if (rdr != null)
            //        {
            //            rdr.Close();
            //        }
            //        if (conn != null)
            //        {
            //            conn.Close();
            //        }
            //    }

            //}
        }
    }
}
