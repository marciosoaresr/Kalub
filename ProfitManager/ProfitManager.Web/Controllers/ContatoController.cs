using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers
{
    public class ContatoController : Controller
    {
        public const string MatchEmailPattern =
       @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
+ @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        public ActionResult Index(string msg, string erro)
        {
            if (msg != null)
            {
                ViewBag.msgenviada = msg;
            }
            if (erro != null)
            {
                ViewBag.erro = erro;
            }
            return View();
        }

        [HttpPost]
        public ActionResult EnviarContato(FormCollection form)
        {
            string nome = form["nome"];
            string email = form["email"];
            string mensagem = form["mensagem"];

            string sUserName = "comercial@kalub.com.br";
            string sPassword = "k28062016";

            if (ValidaEmail(email))
            {
                MailMessage objEmail = new MailMessage();
                objEmail.To.Add("comercial@kalub.com.br");
                objEmail.From = new MailAddress("comercial@kalub.com.br", email);
                objEmail.Subject = "Contato enviado do site";
                objEmail.Body = mensagem;
                objEmail.Priority = System.Net.Mail.MailPriority.Normal;
                objEmail.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.kalub.com.br";
                smtp.Port = 587;

                smtp.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);
                smtp.EnableSsl = false;
                smtp.Send(objEmail);

                string msg = "E-MAIL enviado, aguarde nosso contato!";
                string erro = "";
                return RedirectToAction("Index", new { msg = msg, erro=erro });
            }
            else
            {
                string erro = "Por favor, informe um E-MAIL válido!";
                string msg = "";
                return RedirectToAction("Index", new { erro = erro, msg=msg });
            }


        }

        public static bool ValidaEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }
    }
}
