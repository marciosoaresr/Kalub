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
    public class FuncoesEmpresaAssinaturaIugu
    {
        public static EmpresaAssinaturaIugu Salvar(EmpresaAssinaturaIugu empresaAssinaturaIugu)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(empresaAssinaturaIugu);
                    repositorio.Session.Transaction.Commit();

                    return empresaAssinaturaIugu;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }


        public static List<EmpresaRecebimentoTransacao> LoadExtratoPagseguro(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM EmpresaRecebimentoTransacao e " +
                                 " WHERE Empresa = :idEmpresa" +
                                 " ORDER BY DataHoraCriacao DESC ";

                    var listExtratoPagseguro = repositorio.Buscar<EmpresaRecebimentoTransacao>(hql, parameters).ToList();

                    return listExtratoPagseguro;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
