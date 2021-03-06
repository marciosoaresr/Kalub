using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesExercicio
    {

        public static List<Exercicio> Load(string nomeExercicio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeExercicio", nomeExercicio));
                    string hql = " FROM Exercicio e " +
                                 " WHERE UPPER(TRIM(e.Titulo)) = :nomeExercicio" +
                                 " ORDER BY e.Id ";

                    var listExercicio = repositorio.Buscar<Exercicio>(hql, parameters).ToList();

                    return listExercicio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Exercicio> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    const string hql = "FROM Exercicio ORDER BY Id";
                    var listExercicio = repositorio.Buscar<Exercicio>(hql, null).ToList();

                    return listExercicio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static Exercicio Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));

                    string hql = "FROM Exercicio WHERE Empresa = :id ";

                    var exercicio = repositorio.Buscar<Exercicio>(hql, parameters).FirstOrDefault();

                    return exercicio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Exercicio Salvar(Exercicio exercicio)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    Sincronizar(exercicio, repositorio);
                    repositorio.Salvar(exercicio);

                    foreach (var exercicioOperacao in exercicio.ListExercicioItem)
                    {
                        exercicioOperacao.Data = DateTime.Now;
                        repositorio.Salvar(exercicioOperacao);
                    }

                    repositorio.Session.Transaction.Commit();

                    return exercicio;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
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
                    var exercicio = repositorio.BuscarId<Exercicio>(id);
                    //if (exercicio.ListExercicioItem.Any())
                    //{
                    //    foreach (var exercicioOperacao in exercicio.ListExercicioItem)
                    //    {
                    //        repositorio.Deletar(exercicioOperacao);
                    //    }
                    //}

                    repositorio.Deletar(exercicio);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }


        private static void Sincronizar(Exercicio exercicio, IRepositorio repositorio)
        {
            List<ExercicioItem> listExercicioOperacaoBanco = null;
            if (exercicio.Id != 0)
            {
                var parametrers = new List<HqlParameter>();
                parametrers.Add(new HqlParameter("idExercicio", exercicio.Id));
                string hql = " FROM ExercicioItem WHERE Exercicio = :idExercicio ";
                listExercicioOperacaoBanco = repositorio.Buscar<ExercicioItem>(hql, parametrers).ToList();
            }

            if (listExercicioOperacaoBanco != null && listExercicioOperacaoBanco.Any())
            {
                foreach (var exercicioOperacao in listExercicioOperacaoBanco)
                {
                    if (exercicio.ListExercicioItem.All(x => x.Id != exercicioOperacao.Id && x.Id != 0))
                    {
                        repositorio.Deletar(exercicioOperacao);
                    }
                }
                repositorio.Session.Flush();
                repositorio.Session.Clear();
            }

        }

    }
}
