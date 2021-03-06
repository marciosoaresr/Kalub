using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesCategoriaEmpresaSecundaria
    {
        public static List<CategoriaEmpresaSecundaria> Load(string nomeCategoriaEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeCategoriaEmpresa", nomeCategoriaEmpresa));
                    string hql = " FROM CategoriaEmpresaSecundaria e " +
                                 " INNER JOIN FETCH e.CampoNome cn " +
                                 " WHERE UPPER(TRIM(cn.Nome)) = :nomeCategoriaEmpresa" +
                                 " OR  UPPER(TRIM(cn.Nome)) LIKE '%" + nomeCategoriaEmpresa + "%'" +
                                 " OR e.Codigo " + Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeCategoriaEmpresa);

                    var listCategoriaEmpresa = repositorio.Buscar<CategoriaEmpresaSecundaria>(hql, parameters).ToList();

                    return listCategoriaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<CategoriaEmpresaSecundaria> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM CategoriaEmpresaSecundaria ";

                    var listCategoriaEmpresa = repositorio.Buscar<CategoriaEmpresaSecundaria>(hql, null).ToList();

                    return listCategoriaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<CategoriaEmpresa> LoadByArea(int idArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idArea", idArea));
                    string hql = " FROM Evento WHERE Area = :idArea";

                    var listEvento = repositorio.Buscar<CategoriaEmpresa>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<CategoriaEmpresa> LoadByGrupoArea(int idGrupoArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idGrupoArea", idGrupoArea));
                    string hql = " FROM Evento WHERE Area.GrupoArea = :idGrupoArea ORDER BY Codigo";

                    var listEvento = repositorio.Buscar<CategoriaEmpresa>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static CategoriaEmpresaSecundaria Salvar(CategoriaEmpresaSecundaria categoriaempresasegundaria)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(categoriaempresasegundaria.CampoNome);
                    repositorio.Salvar(categoriaempresasegundaria);
                    repositorio.Session.Transaction.Commit();

                    return categoriaempresasegundaria;
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
