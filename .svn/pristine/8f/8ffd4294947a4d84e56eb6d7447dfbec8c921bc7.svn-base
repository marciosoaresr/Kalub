using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesNaturezaJuridica
    {
        public static List<CategoriaJuridicaEmpresa> Load(string nomeNaturezaJuridica)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeNaturezaJuridica", nomeNaturezaJuridica));
                    string hql = " FROM CategoriaJuridicaEmpresa e " +
                                 " INNER JOIN FETCH e.CampoNome cn " +
                                 " WHERE UPPER(TRIM(cn.Nome)) = :nomeCategoriaEmpresa" +
                                 " OR  UPPER(TRIM(cn.Nome)) LIKE '%" + nomeNaturezaJuridica + "%'" +
                                 " OR e.Codigo " + Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeNaturezaJuridica);

                    var listCategoriaJuridicaEmpresa = repositorio.Buscar<CategoriaJuridicaEmpresa>(hql, parameters).ToList();

                    return listCategoriaJuridicaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<CategoriaJuridicaEmpresa> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM CategoriaJuridicaEmpresa ";

                    var listCategoriaJuridicaEmpresa = repositorio.Buscar<CategoriaJuridicaEmpresa>(hql, null).ToList();

                    return listCategoriaJuridicaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static CategoriaEmpresa Salvar(CategoriaEmpresa categoriaempresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(categoriaempresa.CampoNome);
                    repositorio.Salvar(categoriaempresa);
                    repositorio.Session.Transaction.Commit();

                    return categoriaempresa;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static CategoriaEmpresa Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var categoriaempresa = repositorio.BuscarId<CategoriaEmpresa>(id);

                    return categoriaempresa;
                }
                catch (Exception err)
                {
                    throw err;
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
                    var categoriaempresa = repositorio.BuscarId<CategoriaEmpresa>(id);

                    repositorio.Deletar(categoriaempresa);

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
