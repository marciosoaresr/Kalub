using System;
using System.Linq;
using System.Net;
using System.Web;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.RepositorioWeb
{
    public class RepositorioWebUsuario
    {
        public static bool AutenticarUsuario(string email, string senha, EnumSimNao admin)
        {
            Usuario usuario = null;

            if (email == "kalub.contabil")
            {
                usuario = FuncoesUsuario.AutenticarAdm(email, senha);
            }
            else
            {
                usuario = (admin == EnumSimNao.Sim)
                ? FuncoesUsuario.AutenticarAdm(email, senha)
                : FuncoesUsuario.Autenticar(email, senha);

            }

            if (usuario != null)
            {
                if (email != "kalub.contabil")
                {
                    string nome = Dns.GetHostName();
                    IPAddress[] ip = Dns.GetHostAddresses(nome);
                    var empresaUsuarioLogs = new EmpresaUsuarioLogs();
                    empresaUsuarioLogs.Empresa = Convert.ToInt32(usuario.Empresa.Id);
                    //empresaUsuarioLogs.Ip = ip[2].ToString();
                    empresaUsuarioLogs.Email = email;
                    FuncoesEmpresaUsuarioLogs.Salvar(empresaUsuarioLogs);
                }

                RepositorioWebCookies.RegistrarCookieAutenticacao(usuario.Id, email, usuario.Administrador);
                return true;
            }
            return false;
        }

        public static bool AutenticarCnpj(string cnpj, EnumSimNao admin, string email)
        {
            Usuario usuario = null;
            usuario = FuncoesUsuario.AutenticarCnpj(cnpj);

            if (usuario != null)
            {
                RepositorioWebCookies.RegistrarCookieAutenticacao(usuario.Id, email, usuario.Administrador);
                return true;
            }
            return false;
        }
        public static Usuario RecuperaUsuarioPorId(int idUsuario, EnumSimNao administrador)
        {
            return FuncoesUsuario.BuscarPorId(idUsuario, administrador);
        }

        public static Usuario VerificaSeOUsuarioEstaLogado(bool consultarBanco)
        {
            var usuario = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (usuario == null)
            {
                return null;
            }
            else
            {
                int idUsuario = Convert.ToInt32(RepositorioWebCriptografia.Descriptografar(usuario.Values["IDUsuario"]));
                var administrador = (EnumSimNao)Convert.ToChar((RepositorioWebCriptografia.Descriptografar(usuario.Values["Adm"]).First()));

                var usuarioRetornado = (consultarBanco)
                    ? RecuperaUsuarioPorId(idUsuario, administrador)
                    : new Usuario {Id = idUsuario, Administrador = administrador};

                return usuarioRetornado;
            }
        }

        public static DateTime GetDateQueryCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (cookie == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                string date = cookie.Values["DateQuery"];
                int dia = Convert.ToInt32(date.Substring(0, 2));
                int mes = Convert.ToInt32(date.Substring(3, 2));
                int ano = Convert.ToInt32(date.Substring(6, 4));

                return new DateTime(ano, mes, dia);
            }
        }

        public static void SetDateQueryCookie(DateTime dateTime)
        {
            var cookie = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (cookie == null)
            {
                throw new Exception("Falha set date");
            }
            else
            {
                cookie.Values["DateQuery"] = dateTime.ToString("dd/MM/yyyy");
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        public static void Logout()
        {
            RepositorioWebCookies.Logout();
        }
    }
}