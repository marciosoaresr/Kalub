using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEvento
    {
        public static List<Evento> Load(string nomeEvento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeEvento", nomeEvento));
                    string hql = " FROM Evento e " +
                                 " INNER JOIN FETCH e.CampoNome cn " +
                                 " INNER JOIN e.SubArea sa " +
                                 " WHERE UPPER(TRIM(cn.Nome)) = :nomeEvento" +
                                 " OR  UPPER(TRIM(cn.Nome)) LIKE '%" + nomeEvento + "%'" +
                                 " OR e.Codigo " + Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeEvento) +
                                 " ORDER BY e.Id ";

                    var listEvento = repositorio.Buscar<Evento>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Evento> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Evento ORDER BY Id";

                    var listEvento = repositorio.Buscar<Evento>(hql, null).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Evento> LoadByArea(int idArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idArea", idArea));
                    string hql = " FROM Evento WHERE Area = :idArea";

                    var listEvento = repositorio.Buscar<Evento>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Evento> LoadByGrupoArea(int idGrupoArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idGrupoArea", idGrupoArea));
                    string hql = " FROM Evento WHERE Area.GrupoArea = :idGrupoArea ORDER BY Codigo";

                    var listEvento = repositorio.Buscar<Evento>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static List<Evento> LoadEventosPorTipo(int categoriaEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("categoriaEmpresa", categoriaEmpresa));

                    var subSelect = "(SELECT e.Id " +
                                    "FROM EventoCategoriaEmpresa ec " +
                                    "WHERE ec.Evento = e.Id " +
                                    "AND ec.CategoriaEmpresa = :categoriaEmpresa) ";

                    string hql = " FROM Evento e " +
                                 " WHERE e.RestringeCategoriaEmpresa = 0 " +
                                 " ANd e.Id <> 246 " +  // Remove evento Apuração Saldo
                                 " OR EXISTS " + subSelect + " ORDER BY e.Area";

                    var listEvento = repositorio.Buscar<Evento>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<EventoCategoriaEmpresa> LoadEventosCategoriaEmpresa(int idEvento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEvento", idEvento));
                    string hql = " FROM EventoCategoriaEmpresa WHERE Evento = :idEvento";

                    var listEventoCategoriaEmpresa = repositorio.Buscar<EventoCategoriaEmpresa>(hql, parameters).ToList();

                    return listEventoCategoriaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Evento Salvar(Evento evento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(evento.CampoNome);
                    repositorio.Salvar(evento.CampoHelp);

                    Sincronizar(evento, repositorio);
                    SincronizarCategoriaEmpresa(evento, repositorio);

                    repositorio.Salvar(evento);

                    foreach (var eventoOperacao in evento.ListEventoOperacao)
                    {
                        repositorio.Salvar(eventoOperacao);
                    }
                    foreach (var eventoCategoria in evento.ListEventoCategoriaEmpresa)
                    {
                        repositorio.Salvar(eventoCategoria);
                    }

                    repositorio.Session.Transaction.Commit();

                    return evento;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Evento SalvarHelp(Evento evento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {

                    repositorio.Salvar(evento);

                    repositorio.Session.Transaction.Commit();

                    return evento;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static Evento Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var evento = repositorio.BuscarId<Evento>(id);

                    return evento;
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
                    var evento = repositorio.BuscarId<Evento>(id);
                    if (evento.ListEventoOperacao.Any())
                    {
                        foreach (var eventoOperacao in evento.ListEventoOperacao)
                        {
                            repositorio.Deletar(eventoOperacao);
                        }
                    }
                    if (evento.ListEventoCategoriaEmpresa.Any())
                    {
                        foreach (var eventoCategoriaEmpresa in evento.ListEventoCategoriaEmpresa)
                        {
                            repositorio.Deletar(eventoCategoriaEmpresa);
                        }
                    }

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

        private static void Sincronizar(Evento evento, IRepositorio repositorio)
        {
            List<EventoOperacao> listEventoOperacaoBanco = null;
            if (evento.Id != 0)
            {
                var parametrers = new List<HqlParameter>();
                parametrers.Add(new HqlParameter("idEvento", evento.Id));
                string hql = " FROM EventoOperacao WHERE Evento = :idEvento ";
                listEventoOperacaoBanco = repositorio.Buscar<EventoOperacao>(hql, parametrers).ToList();
            }

            if (listEventoOperacaoBanco != null && listEventoOperacaoBanco.Any())
            {
                foreach (var eventoOperacao in listEventoOperacaoBanco)
                {
                    if (evento.ListEventoOperacao.All(x => x.Id != eventoOperacao.Id && x.Id != 0))
                    {
                        repositorio.Deletar(eventoOperacao);
                    }
                }
                repositorio.Session.Flush();
                repositorio.Session.Clear();
            }

        }

        private static void SincronizarCategoriaEmpresa(Evento evento, IRepositorio repositorio)
        {
            List<EventoCategoriaEmpresa> listEventoCategoriaEmpresa = null;
            if (evento.Id != 0)
            {
                var parametrers = new List<HqlParameter>();
                parametrers.Add(new HqlParameter("idEvento", evento.Id));
                string hql = " FROM EventoCategoriaEmpresa WHERE Evento = :idEvento ";
                listEventoCategoriaEmpresa = repositorio.Buscar<EventoCategoriaEmpresa>(hql, parametrers).ToList();
            }

            if (listEventoCategoriaEmpresa != null && listEventoCategoriaEmpresa.Any())
            {
                foreach (var eventoCategoriaEmpresa in listEventoCategoriaEmpresa)
                {
                    if (evento.ListEventoCategoriaEmpresa.All(x => x.Id != eventoCategoriaEmpresa.Id && x.Id != 0))
                    {
                        repositorio.Deletar(eventoCategoriaEmpresa);
                    }
                }
                repositorio.Session.Flush();
                repositorio.Session.Clear();
            }

        }

        public static List<Evento> LoadEventosPorExercicio(int categoriaEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("categoriaEmpresa", categoriaEmpresa));

                    var subSelect = "(SELECT e.Id " +
                                    "FROM ExercicioItem ei " +
                                    "WHERE ei.CategoriaEmpresa = :categoriaEmpresa) ";

                    string hql = " FROM Evento e " +
                                 " WHERE e.RestringeCategoriaEmpresa = 0 " +
                                 " ANd e.Id <> 246 " +  // Remove evento Apuração Saldo
                                 " OR EXISTS " + subSelect + " ORDER BY e.Area";

                    var listEvento = repositorio.Buscar<Evento>(hql, parameters).ToList();

                    return listEvento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
