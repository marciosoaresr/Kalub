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
    public class FuncoesTermoUso
    {

        public static List<TermoUso> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM TermoUso ";

                    var listTermo = repositorio.Buscar<TermoUso>(hql, null).ToList();

                    return listTermo;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static TermoUso Salvar(TermoUso termouso)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(termouso);
                    repositorio.Session.Transaction.Commit();

                    return termouso;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static TermoUso Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var termouso = repositorio.BuscarId<TermoUso>(id);

                    return termouso;
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
                    var termouso = repositorio.BuscarId<TermoUso>(id);

                    repositorio.Deletar(termouso);

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
