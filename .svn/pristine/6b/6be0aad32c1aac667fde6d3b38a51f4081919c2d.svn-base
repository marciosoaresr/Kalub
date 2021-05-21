using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesExercicioEmpresa
    {


        public static ExercicioEmpresa Salvar(ExercicioEmpresa exercicioempresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(exercicioempresa);

                    repositorio.Session.Transaction.Commit();

                    return exercicioempresa;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<ExercicioEmpresa> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    const string hql = "FROM ExercicioEmpresa ORDER BY Id";
                    var listExercicio = repositorio.Buscar<ExercicioEmpresa>(hql, null).ToList();

                    return listExercicio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ExercicioEmpresa Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var exercicioEmpresa = repositorio.BuscarId<ExercicioEmpresa>(id);

                    return exercicioEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ExercicioEmpresa DeletarExercicioEmpresa(int idExercicio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idExercicio", idExercicio));
                    var hqlSaldoInicialExistes = " FROM ExercicioEmpresa" +
                                                " WHERE exercicio = :idExercicio ";
                    var exercicioEmpresa = repositorio.Buscar<ExercicioEmpresa>(hqlSaldoInicialExistes, parameters);
                    foreach (var exercicio in exercicioEmpresa)
                    {
                        repositorio.Deletar(exercicio);
                    }
                    repositorio.Session.Transaction.Commit();
                    return null;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw err;
                }
            }
        }
    }
}
