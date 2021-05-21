using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesContaContabilGrupo
    {

        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var contaContabilGrupo = repositorio.BuscarId<ContaContabilGrupo>(id);

                    if (contaContabilGrupo.ListContaContabilSubGrupo.Any())
                    {
                        foreach (var contaContabilSubGrupo in contaContabilGrupo.ListContaContabilSubGrupo)
                        {
                            if (contaContabilSubGrupo.ListContaContabil.Any())
                            {
                                foreach (var contaContabil in contaContabilSubGrupo.ListContaContabil)
                                {
                                    repositorio.Deletar(contaContabil);
                                }
                            }
                            repositorio.Deletar(contaContabilSubGrupo);
                        }
                    }

                    repositorio.Deletar(contaContabilGrupo);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static ContaContabilGrupo Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parametrers = new List<HqlParameter>();
                    parametrers.Add(new HqlParameter("idContaContabilGrupo", id));

                    const string hql = "FROM ContaContabilGrupo WHERE Id = :idContaContabilGrupo ";

                    var contaContabilGrupo = repositorio.Buscar<ContaContabilGrupo>(hql, parametrers).FirstOrDefault();

                    return contaContabilGrupo;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<ContaContabilGrupo> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM ContaContabilGrupo ";

                    var listContaContabilGrupo = repositorio.Buscar<ContaContabilGrupo>(hql, null).ToList();

                    return listContaContabilGrupo;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ContaContabilGrupo Salvar(ContaContabilGrupo contaContabilGrupo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(contaContabilGrupo.CampoNome);
                    repositorio.Salvar(contaContabilGrupo);
                    Thread.Sleep(1000);

                    repositorio.Session.Transaction.Commit();

                    return contaContabilGrupo;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Decimal PegaSaldoGrupoDia(int idEmpresa, DateTime dataLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    parameters.Add(new HqlParameter("dataLancamento", dataLancamento));
                    parameters.Add(new HqlParameter("grupo", "3"));

                    const string hql = "SELECT SUM(ccs.Saldo) as Saldo " +
                                 "FROM ContaContabilSaldo ccs " +
                                 "INNER JOIN  ccs.ContaContabil cc " +
                                 "INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                 "INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                 "WHERE ccs.Data = :dataLancamento " +
                                 "AND ccs.Empresa = :idEmpresa " + 
                                 "AND ccg.Codigo = :grupo";

                    var result = repositorio.SeekByHqlObject(hql, parameters);
                    var valor = Convert.ToDecimal(result[0][0]);

                    return valor;


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
