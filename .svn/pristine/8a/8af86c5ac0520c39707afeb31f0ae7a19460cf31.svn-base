using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEmpresaTransacaoIugu
    {
        public static EmpresaTransacaoIugu Salvar(EmpresaTransacaoIugu empresaTransacaoIugu)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(empresaTransacaoIugu);
                    repositorio.Session.Transaction.Commit();

                    return empresaTransacaoIugu;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<EmpresaTransacaoIugu> LoadFaturas(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM EmpresaTransacaoIugu e " +
                                 " WHERE Empresa = :idEmpresa" +
                                 " ORDER BY DataHoraCriacao DESC ";

                    var listFaturas = repositorio.Buscar<EmpresaTransacaoIugu>(hql, parameters).ToList();

                    return listFaturas;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static EmpresaTransacaoIugu CarregaFatura(string idFatura)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idFatura", idFatura));
                    const string hql = "FROM EmpresaTransacaoIugu WHERE IdFatura = :idFatura ";

                    var fatura = repositorio.Buscar<EmpresaTransacaoIugu>(hql, parameters).SingleOrDefault();

                    return fatura;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
