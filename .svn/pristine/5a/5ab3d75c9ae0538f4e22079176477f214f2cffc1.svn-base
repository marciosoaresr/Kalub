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
    public class FuncoesManual
    {

        public static List<Manual> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Manual ";

                    var listManual = repositorio.Buscar<Manual>(hql, null).ToList();

                    return listManual;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static Manual Salvar(Manual manual)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(manual);
                    repositorio.Session.Transaction.Commit();

                    return manual;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Manual Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var manual = repositorio.BuscarId<Manual>(id);

                    return manual;
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
                    var manual = repositorio.BuscarId<Manual>(id);

                    repositorio.Deletar(manual);

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
