using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.EntidadeAuxiliar;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesDiagnostico
    {
        public static List<Diagnostico> Load(string nomeGrupoDre)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeGrupoDRE", nomeGrupoDre));
                    string hql = " FROM Diagnostico e " +
                                 " WHERE UPPER(TRIM(Nome)) = :nomeGrupoDRE" +
                                 " OR  UPPER(TRIM(Nome)) LIKE '%" + nomeGrupoDre + "%'";

                    var listGrupoDre = repositorio.Buscar<Diagnostico>(hql, parameters).ToList();

                    return listGrupoDre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Diagnostico> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Diagnostico ";

                    var listGrupoDre = repositorio.Buscar<Diagnostico>(hql, null).ToList();

                    return listGrupoDre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static Diagnostico Salvar(Diagnostico diagnostico)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(diagnostico);
                    repositorio.Session.Transaction.Commit();

                    return diagnostico;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Diagnostico Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var diagnostico = repositorio.BuscarId<Diagnostico>(id);

                    return diagnostico;
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
                    var diagnostico = repositorio.BuscarId<Diagnostico>(id);

                    repositorio.Deletar(diagnostico);

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
