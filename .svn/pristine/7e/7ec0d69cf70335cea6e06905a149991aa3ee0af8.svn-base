using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesEmail
    {

        public static void EnviarEmail(string destinatario, EnumTemplateEmail tipoEmail, string senha)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;

                string template = "";
                string assunto = "";
                if (tipoEmail == EnumTemplateEmail.EmailBoasVindas)
                {
                    assunto = "Seja muito bem-vindo ao KALUB! Vamos começar?";
                    template = "/EmailBoasVindas.html";
                }
                if (tipoEmail == EnumTemplateEmail.PagtoAprovadoPagseguro)
                {
                    assunto = "KALUB - Confirmação de Pagamento";
                    template = "/EmailPagamentoAprovadoPagseguro.html";
                }
                if (tipoEmail == EnumTemplateEmail.EmailRecuperaSenha)
                {
                    assunto = "KALUB - Recuperação de Senha";
                    template = "/EmailRecuperarSenha.html";
                }
                string sTemplate = wc.DownloadString("http://www.kalub.com.br" + template);

                if (senha != null)
                {
                    sTemplate = sTemplate.Replace("##Senha##", senha);
                    sTemplate = sTemplate.Replace("##Destinatario##", destinatario);
                }

                string sUserName = "no-reply@kalub.com.br";
                string sPassword = "k28062016";

                MailMessage objEmail = new MailMessage();
                objEmail.To.Add(destinatario);
                objEmail.From = new MailAddress("no-reply@kalub.com.br");
                objEmail.Subject = assunto;
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
            catch (Exception err)
            {
                throw err;
            }
        }


    }
}
