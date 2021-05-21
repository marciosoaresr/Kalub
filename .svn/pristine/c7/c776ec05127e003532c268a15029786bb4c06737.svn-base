using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesTipoDocumentoFechamentoCaixa
    {
        public static List<TipoDocumentoFechamentoCaixa> Load(string nomeTipoDocumento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeCategoriaEmpresa", nomeTipoDocumento));
                    string hql = " FROM TipoDocumentoFechamentoCaixa e " +
                                 " INNER JOIN FETCH e.CampoNome cn " +
                                 " WHERE UPPER(TRIM(cn.Nome)) = :nomeCategoriaEmpresa" +
                                 " OR  UPPER(TRIM(cn.Nome)) LIKE '%" + nomeTipoDocumento + "%'" +
                                 " OR e.Codigo " + Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeTipoDocumento);

                    var listTipoDocumento = repositorio.Buscar<TipoDocumentoFechamentoCaixa>(hql, parameters).ToList();

                    return listTipoDocumento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<TipoDocumentoFechamentoCaixa> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM TipoDocumentoFechamentoCaixa ";

                    var listTipoDocumento = repositorio.Buscar<TipoDocumentoFechamentoCaixa>(hql, null).ToList();

                    return listTipoDocumento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }



        public static TipoDocumentoFechamentoCaixa Salvar(TipoDocumentoFechamentoCaixa tipodocumentofechamentocaixa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(tipodocumentofechamentocaixa.CampoNome);
                    repositorio.Salvar(tipodocumentofechamentocaixa);
                    repositorio.Session.Transaction.Commit();

                    return tipodocumentofechamentocaixa;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static TipoDocumentoFechamentoCaixa Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var categoriaempresa = repositorio.BuscarId<TipoDocumentoFechamentoCaixa>(id);

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
                    var categoriaempresa = repositorio.BuscarId<TipoDocumentoFechamentoCaixa>(id);

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
