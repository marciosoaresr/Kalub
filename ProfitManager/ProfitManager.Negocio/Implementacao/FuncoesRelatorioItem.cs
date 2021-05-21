using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesRelatorioItem
    {

        public static RelatorioItem Load(int idRelatorioItem)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var relatorioItem = repositorio.BuscarId<RelatorioItem>(idRelatorioItem);

                    return relatorioItem;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<RelatorioItem> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM RelatorioItem ";

                    var listRelatorioItem = repositorio.Buscar<RelatorioItem>(hql, null).ToList();

                    return listRelatorioItem;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<RelatorioItem> Load(string nomeOucodigo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeOuCodigo", nomeOucodigo.ToUpper()));

                    string hql = "FROM RelatorioItem WHERE Codigo " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo) + " OR UPPER(Nome) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo.ToUpper()) +
                                 " OR UPPER(Nome) = :nomeOuCodigo ";

                    var listRelatorioItem = repositorio.Buscar<RelatorioItem>(hql, parameters).ToList();

                    return listRelatorioItem;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<RelatorioItem> Load(string nomeOucodigo, int idRelatorio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idRelatorio", idRelatorio));
                    parameters.Add(new HqlParameter("nomeOuCodigo", nomeOucodigo.ToUpper()));

                    string hql = "FROM RelatorioItem WHERE (Codigo " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo) + " OR UPPER(Nome) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo.ToUpper()) +
                                 " OR UPPER(Nome) = :nomeOuCodigo) AND Relatorio = :idRelatorio ";

                    var listRelatorioItem = repositorio.Buscar<RelatorioItem>(hql, parameters).ToList();

                    return listRelatorioItem;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static RelatorioItem Salvar(RelatorioItem relatorioItem)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    Sincronizar(relatorioItem, repositorio);
                    
                    repositorio.Salvar(relatorioItem);

                    foreach (var contaContabilRelItem in relatorioItem.ListContaContabilRelatorioItem)
                    {
                        repositorio.Salvar(contaContabilRelItem);
                    }

                    repositorio.Session.Transaction.Commit();

                    return relatorioItem;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        private static void Sincronizar(RelatorioItem relItem, IRepositorio repositorio)
        {
            List<ContaContabilRelatorioItem> listContaContabilRelItem = null;
            if (relItem.Id != 0)
            {
                var parameters = new List<HqlParameter>();
                parameters.Add(new HqlParameter("idRelatorioItem", relItem.Id));
                string hql = " FROM ContaContabilRelatorioItem WHERE RelatorioItem = :idRelatorioItem";
                listContaContabilRelItem = repositorio.Buscar<ContaContabilRelatorioItem>(hql, parameters).ToList();
            }

            if (listContaContabilRelItem != null && listContaContabilRelItem.Any())
            {
                foreach (var relatorioItem in listContaContabilRelItem)
                {
                    if (relItem.ListContaContabilRelatorioItem.All(x => (x.Id != relatorioItem.Id) && x.Id != 0))
                    {
                        repositorio.Deletar(relatorioItem);
                    }
                }

                repositorio.Session.Flush();
                repositorio.Session.Clear();
            }
        }

        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var relItem = repositorio.BuscarId<RelatorioItem>(id);

                    if (relItem.ListContaContabilRelatorioItem.Any())
                    {
                        foreach (var contaContabilRelItem in relItem.ListContaContabilRelatorioItem)
                        {
                            repositorio.Deletar(contaContabilRelItem);
                        }
                    }

                    repositorio.Deletar(relItem);

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
