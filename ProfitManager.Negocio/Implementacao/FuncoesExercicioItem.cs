using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesExercicioItem
    {
        public static ExercicioItem Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var eventoExercicio = repositorio.BuscarId<ExercicioItem>(id);

                    return eventoExercicio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ExercicioItem Salvar(ExercicioItem exercicioitem)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(exercicioitem);

                    repositorio.Session.Transaction.Commit();

                    return exercicioitem;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }

            }
        }

        public static List<ExercicioItem> LoadExercicioItem(int idExercicio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idExercicio", idExercicio));
                    string hql = " FROM ExercicioItem WHERE Exercicio = :idExercicio";

                    var listExercicioItem = repositorio.Buscar<ExercicioItem>(hql, parameters).ToList();

                    return listExercicioItem;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ExercicioItem DeletarExercicioItem(int idExercicio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idExercicio", idExercicio));

                    var hqlSaldoInicialExistes = " FROM ExercicioItem" +
                                                " WHERE exercicio = :idExercicio ";

                    var exercicioItem = repositorio.Buscar<ExercicioItem>(hqlSaldoInicialExistes, parameters);

                    foreach (var exercicio in exercicioItem)
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