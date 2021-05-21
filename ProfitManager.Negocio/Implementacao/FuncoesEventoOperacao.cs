using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEventoOperacao
    {
        public static EventoOperacao Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var eventoOperacao = repositorio.BuscarId<EventoOperacao>(id);

                    return eventoOperacao;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static List<EventoOperacao> LoadContasEventoOperacao(int idEvento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEvento", idEvento));


                    string hql = " FROM EventoOperacao  " +
                                 " WHERE Evento = :idEvento";

                    var listContaContabil = repositorio.Buscar<EventoOperacao>(hql, parameters).ToList();

                    return listContaContabil;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
