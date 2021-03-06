using System;
using System.Web;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Web.RepositorioWeb
{
    public class RepositorioWebCookies
    {
        public static void RegistrarCookieAutenticacao(int idUsuario, string email, EnumSimNao administrador)
        {
            //Criando um objeto cookie
            var userCookie = new HttpCookie("UserCookieAuthentication");

            //Setando o ID do usuário no cookie
            userCookie.Values["IDUsuario"] = RepositorioWebCriptografia.Criptografar(idUsuario.ToString());
            userCookie.Values["Login"] = email;
            userCookie.Values["Adm"] = RepositorioWebCriptografia.Criptografar(administrador.ToString());
            userCookie.Values["DateQuery"] = DateTime.MinValue.ToString("dd/MM/yyyy");
            
            //Definindo o prazo de vida do cookie
            userCookie.Expires = DateTime.Now.AddDays(1);

            //Adicionando o cookie no contexto da aplicação
            HttpContext.Current.Response.Cookies.Add(userCookie);
        }

        public static void Logout()
        {
            var usuario = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (usuario != null)
            {
                usuario.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(usuario);
            }
        }
    }
}