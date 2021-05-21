using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesParametros
    {

        public static List<Parametros> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Parametros ";

                    var listParametros = repositorio.Buscar<Parametros>(hql, null).ToList();

                    return listParametros;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static Parametros Salvar(Parametros parametros)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(parametros);
                    repositorio.Session.Transaction.Commit();

                    return parametros;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Parametros Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parametros = repositorio.BuscarId<Parametros>(id);

                    return parametros;
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
                    var parametros = repositorio.BuscarId<Parametros>(id);

                    repositorio.Deletar(parametros);

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
