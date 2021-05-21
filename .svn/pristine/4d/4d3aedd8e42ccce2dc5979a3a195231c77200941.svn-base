using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesContaContabilSubGrupo
    {
        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var contaContabilSubGrupo = repositorio.BuscarId<ContaContabilSubGrupo>(id);

                    if (contaContabilSubGrupo.ListContaContabil.Any())
                    {
                        foreach (var contaContabil in contaContabilSubGrupo.ListContaContabil)
                        {
                            repositorio.Deletar(contaContabil);   
                        }
                    }

                    repositorio.Deletar(contaContabilSubGrupo);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static ContaContabilSubGrupo Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parametrers = new List<HqlParameter>();
                    parametrers.Add(new HqlParameter("idContaContabilSubGrupo", id));

                    const string hql = "FROM ContaContabilSubGrupo WHERE Id = :idContaContabilSubGrupo ";

                    var contaContabilSubGrupo = repositorio.Buscar<ContaContabilSubGrupo>(hql, parametrers).FirstOrDefault();

                    return contaContabilSubGrupo;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<ContaContabilSubGrupo> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM ContaContabilSubGrupo ";

                    var listContaContabilSubGrupo = repositorio.Buscar<ContaContabilSubGrupo>(hql, null).ToList();

                    return listContaContabilSubGrupo;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ContaContabilSubGrupo Salvar(ContaContabilSubGrupo contaContabilSubGrupo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(contaContabilSubGrupo.CampoNome);
                    repositorio.Salvar(contaContabilSubGrupo);

                    repositorio.Session.Transaction.Commit();

                    return contaContabilSubGrupo;
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
