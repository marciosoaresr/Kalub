using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.EntidadeAuxiliar;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesRelatorio
    {

        public static Relatorio Salvar(Relatorio relatorio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(relatorio);

                    repositorio.Session.Transaction.Commit();

                    return relatorio;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Relatorio Load(int idRelatorio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idRelatorio", idRelatorio));
                    const string hql = "FROM Relatorio WHERE Id = :idRelatorio ";

                    var relatorio = repositorio.Buscar<Relatorio>(hql, parameters).SingleOrDefault();

                    return relatorio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Relatorio> Load(string nomeOucodigo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeOuCodigo", nomeOucodigo.ToUpper()));

                    string hql = "FROM Relatorio WHERE Codigo " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo) + " OR UPPER(Nome) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeOucodigo.ToUpper()) +
                                 " OR UPPER(Nome) = :nomeOuCodigo ";

                    var listRelatorio = repositorio.Buscar<Relatorio>(hql, parameters).ToList();

                    return listRelatorio;
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
                    var evento = repositorio.BuscarId<Relatorio>(id);

                    repositorio.Deletar(evento);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<Relatorio> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Relatorio";

                    var listRelatorio = repositorio.Buscar<Relatorio>(hql, null).ToList();

                    return listRelatorio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

       


    }
}
