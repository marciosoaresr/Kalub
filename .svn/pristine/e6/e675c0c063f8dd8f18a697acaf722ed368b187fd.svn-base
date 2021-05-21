using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesEmpresaUsuario
    {
        public static EmpresaUsuario Load(int idEmpresaUsuario)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresaUsuario", idEmpresaUsuario));

                    string hql = "FROM EmpresaUsuario WHERE Id = :idEmpresaUsuario ";

                    var empresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    return empresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<EmpresaUsuario> Carrega(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));
                    string hql = " FROM EmpresaUsuario e " +
                                 " WHERE e.Empresa = :id";

                    var listEmpresa = repositorio.Buscar<EmpresaUsuario>(hql, parameters).ToList();

                    return listEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static Empresa BuscarEmpresaEmail(string email)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("email", email));
                    string hql = " FROM Empresa WHERE Email = :email";

                    var listDadosEmpresa = repositorio.Buscar<Empresa>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static Empresa BuscarEmpresa(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM Empresa WHERE Id = :idEmpresa";

                    var listDadosEmpresa = repositorio.Buscar<Empresa>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static EmpresaUsuario BuscarEmpresaUsuario(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));
                    string hql = " FROM EmpresaUsuario WHERE Id = :id";

                    var listDadosEmpresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static EmpresaUsuario BuscarEmpresaUsuarioAdm(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));
                    string hql = " FROM EmpresaUsuario WHERE Empresa = :id";

                    var listDadosEmpresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static EmpresaUsuario BuscarEmpresaUsuarioEmail(string email)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("email", email));
                    string hql = " FROM EmpresaUsuario WHERE Email = :email";

                    var listDadosEmpresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static EmpresaUsuario Salvar(EmpresaUsuario empresaUsuario)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Session.Clear();
                    repositorio.Salvar(empresaUsuario);

                    repositorio.Session.Transaction.Commit();

                    return empresaUsuario;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var empresausuario = repositorio.BuscarId<EmpresaUsuario>(id);

                    repositorio.Deletar(empresausuario);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<EmpresaUsuario> EmailExistente(string email)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("email", email));
                    string hql = " FROM EmpresaUsuario e " +
                                 " WHERE e.Email = :email";

                    var listEmpresa = repositorio.Buscar<EmpresaUsuario>(hql, parameters).ToList();

                    return listEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
