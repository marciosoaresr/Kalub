using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesEmpresaUsuarioLogs
    {
        public static EmpresaUsuarioLogs Load(int idEmpresaUsuario)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresaUsuario", idEmpresaUsuario));

                    string hql = "FROM EmpresaUsuarioLogs WHERE Id = :idEmpresaUsuario ";

                    var empresaUsuario = repositorio.Buscar<EmpresaUsuarioLogs>(hql, parameters).FirstOrDefault();

                    return empresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<EmpresaUsuarioLogs> Carrega(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));
                    string hql = " FROM EmpresaUsuarioLogs e " +
                                 " WHERE e.Empresa = :id ORDER BY e.DataHoraCriacao DESC";

                    var listEmpresa = repositorio.Buscar<EmpresaUsuarioLogs>(hql, parameters).ToList();

                    return listEmpresa;
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

        public static EmpresaUsuarioLogs BuscarEmpresaUsuarioLogs(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM EmpresaUsuarioLogs WHERE Empresa = :idEmpresa";

                    var listDadosEmpresaUsuario = repositorio.Buscar<EmpresaUsuarioLogs>(hql, parameters).FirstOrDefault();

                    return listDadosEmpresaUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static EmpresaUsuarioLogs Salvar(EmpresaUsuarioLogs empresaUsuarioLogs)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Session.Clear();
                    repositorio.Salvar(empresaUsuarioLogs);

                    repositorio.Session.Transaction.Commit();

                    return empresaUsuarioLogs;
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
                    var empresa = repositorio.BuscarId<EmpresaUsuarioLogs>(id);

                    repositorio.Deletar(empresa);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
