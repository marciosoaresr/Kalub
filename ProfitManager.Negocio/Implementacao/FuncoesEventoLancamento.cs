
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesEventoLancamento
    {
        public static EventoLancamento Salvar(EventoLancamento eventoLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(eventoLancamento);

                    repositorio.Session.Transaction.Commit();

                    return eventoLancamento;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<EventoLancamento> Salvar(List<EventoLancamento> listEventoLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    foreach (var eventoLancamento in listEventoLancamento)
                    {
                        repositorio.Salvar(eventoLancamento);
                    }

                    repositorio.Session.Transaction.Commit();

                    return listEventoLancamento;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<EventoLancamento> LoadById(List<int> listIdEventoLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();

                    //                       0        1          2               3            4         5
                    string hql = " SELECT evl.Id, evl.Valor, evl.Data, evl.DataHoraCriacao, e.Id,  e.Codigo, " +
                                 //              6            7             8       9        10           11
                                 " ecampo.Nome, ecampohelp.Nome, emp.Id, a.Id, acampo.Nome, a.Codigo   " +
                                 " FROM EventoLancamento evl " +
                                 " INNER JOIN evl.Evento e " +
                                 " INNER JOIN e.Area a " +
                                 " INNER JOIN a.CampoNome acampo " +
                                 " INNER JOIN e.CampoNome ecampo " +
                                 " INNER JOIN e.CampoHelp ecampohelp " +
                                 " INNER JOIN evl.Empresa emp " +
                                 " WHERE 1 = 1 ";

                    Funcoes.HqlParametroIn(listIdEventoLancamento.ToArray(), ref hql, ref parameters, "evl.Id");

                    var listEventoLancamentoObject = repositorio.SeekByHqlObject(hql, parameters);

                    List<EventoLancamento> listEventoLancamento =
                        listEventoLancamentoObject.Select(evnt => new EventoLancamento
                        {
                            Id = Convert.ToInt32(evnt[0]),
                            Valor = Convert.ToDecimal(evnt[1]),
                            Data = Convert.ToDateTime(evnt[2]),
                            DataHoraCriacao = Convert.ToDateTime(evnt[3]),
                            Evento = new Evento
                            {
                                Id = Convert.ToInt32(evnt[4]),
                                Codigo = Convert.ToString(evnt[5]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(evnt[6])
                                },
                                CampoHelp = new Campo
                                {
                                    Nome = Convert.ToString(evnt[7])
                                },
                                Area = new Area
                                {
                                    Codigo = evnt[11] as string,
                                    Id = Convert.ToInt32(evnt[9]),
                                    CampoNome = new Campo
                                    {
                                        Nome = evnt[10] as string
                                    }
                                }
                            },
                            Empresa = new Empresa
                            {
                                Id = Convert.ToInt32(evnt[8])
                            }

                        }).ToList();

                    return listEventoLancamento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static List<EventoLancamento> LoadByGrupoAreaAndEmpresaAndData(List<Evento> eventos, int idEmpresa, DateTime dataHoraLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var lancamentos = new List<EventoLancamento>();

                    var lancamentosDiarios = BuscarLancamentos(repositorio, dataHoraLancamento, idEmpresa,
                        eventos.Where(x => x.TipoEventoLancamento == EnumTipoEventoLancamento.Diario)
                            .Select(x => x.Id)
                            .ToList());

                    var lancamentosMensais = BuscarLancamentos(repositorio, new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month, 1), idEmpresa,
                        eventos.Where(x => x.TipoEventoLancamento == EnumTipoEventoLancamento.Mensal)
                            .Select(x => x.Id)
                            .ToList());


                    lancamentos.AddRange(lancamentosDiarios);
                    lancamentos.AddRange(lancamentosMensais);

                    return lancamentos;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<EventoLancamento> BuscarLancamentos(IRepositorio repositorio, DateTime dataHoraLancamento,
            int idEmpresa, List<int> eventosId)
        {
            var parameters = new List<HqlParameter>();
            parameters.Add(new HqlParameter("dataLancamento", dataHoraLancamento.Date));
            parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
            var dataPrimeiroDiaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month, 1);
            var dataUltimaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month,
                DateTime.DaysInMonth(dataHoraLancamento.Year, dataHoraLancamento.Month));
             
            parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
            parameters.Add(new HqlParameter("ultimoDiaMes", dataHoraLancamento));


            var subSelectLancamentoMes = "(SELECT SUM(evl1.Valor) " +
                                         "FROM EventoLancamento evl1 " +
                                         "INNER JOIN evl1.Evento e1 " +
                                         "WHERE e1.Id = e.Id " +
                                         "AND evl1.Data BETWEEN :primeiroDiaMes AND :ultimoDiaMes " +
                                         "AND evl1.Empresa = :idEmpresa)";

            //                       0        1          2               3            4         5
            string hql = " SELECT evl.Id, evl.Valor, evl.Data, evl.DataHoraCriacao, e.Id,  e.Codigo, " +
                         //               6            7            8       9       10          11
                         " ecampo.Nome, ecampohelp.Nome, emp.Id, a.Id, acampo.Nome, a.Codigo, e.TipoEventoLancamento, evl.NotaExplicativa, " +
                         subSelectLancamentoMes + ", e.MaisUsado, e.Help, sa.Id, sa.Codigo, sa.Nome, e.Ordem  " +
                         " FROM EventoLancamento evl " +
                         " INNER JOIN evl.Evento e " +
                         " INNER JOIN e.CampoNome ecampo " +
                         " INNER JOIN e.CampoHelp ecampohelp " +
                         " INNER JOIN evl.Empresa emp " +
                         " INNER JOIN e.Area a " +
                         " LEFT JOIN e.SubArea sa " +
                         " INNER JOIN a.CampoNome acampo " +
                         " WHERE emp.Id = :idEmpresa " +
                         " AND evl.Data = :dataLancamento ";

            Funcoes.HqlParametroIn(eventosId.ToArray(), ref hql, ref parameters, "e.Id");
            hql += "ORDER BY e.Codigo ";
            var listEventoLancamentoObject = repositorio.SeekByHqlObject(hql, parameters);


            List<EventoLancamento> listEventoLancamento =
                listEventoLancamentoObject.Select(evnt => new EventoLancamento
                {
                    Id = Convert.ToInt32(evnt[0]),
                    Valor = Convert.ToDecimal(evnt[1]),
                    Data = Convert.ToDateTime(evnt[2]),
                    NotaExplicativa = (evnt[13]) as string,
                    ValorMes = Convert.ToDecimal(evnt[14]),
                    DataHoraCriacao = Convert.ToDateTime(evnt[3]),
                    Evento = new Evento
                    {
                        Id = Convert.ToInt32(evnt[4]),
                        Codigo = Convert.ToString(evnt[5]),
                        TipoEventoLancamento = (EnumTipoEventoLancamento)Convert.ToChar(evnt[12]),
                        MaisUsado = (EnumMaisUsado)Convert.ToChar(evnt[15]),
                        Help = Convert.ToString(evnt[16]),
                        CampoNome = new Campo
                        {
                            Nome = Convert.ToString(evnt[6])
                        },
                        CampoHelp = new Campo
                        {
                            Nome = Convert.ToString(evnt[7])
                        },
                        Area = new Area
                        {
                            Codigo = evnt[11] as string,
                            Id = Convert.ToInt32(evnt[9]),
                            CampoNome = new Campo
                            {
                                Nome = evnt[10] as string
                            }
                        },
                        SubArea = new SubArea
                        {
                            Id = Convert.ToInt32(evnt[17]),
                            Codigo = evnt[18] as string,
                            Nome = evnt[19] as string
                        },
                        Ordem = Convert.ToInt32(evnt[20])
                    },
                    Empresa = new Empresa
                    {
                        Id = Convert.ToInt32(evnt[8])
                    }

                }).ToList();

            return listEventoLancamento;
        }

        public static decimal PegaSaldoInicialContaContabil(int idEmpresa, int contaContabil, DateTime data)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                return PegaSaldoInicialContaContabilExec (repositorio, idEmpresa, contaContabil, data, false);
            }
        }

        public static decimal PegaSaldoInicialContaContabil(int idEmpresa, int contaContabil, DateTime data, bool convertido)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                var conta = repositorio.BuscarId<ContaContabil>(contaContabil);
                decimal decValor = PegaSaldoInicialContaContabilExec(repositorio, idEmpresa, contaContabil, data, convertido);
                if (conta.TipoContaContabil.GetValueOrDefault(EnumTipoContaContabil.Credora) ==
                    EnumTipoContaContabil.Devedora) decValor = decValor*-1;
                return decValor;
            }
        }

        private static decimal PegaSaldoInicialContaContabilExec(Repositorio.Repositorio repositorio, int idEmpresa, int contaContabil, DateTime data, bool convertido)
        {

            try
            {
                var parameters = new List<HqlParameter>();
                parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                parameters.Add(new HqlParameter("idConta", contaContabil));

                decimal decValor = 0;

                parameters.Add(new HqlParameter("data", data.Date));
                string hql = " SELECT Sum(cs.Saldo) " +
                             " FROM ContaContabilSaldo cs " +
                             " INNER JOIN cs.ContaContabil cc " +
                             " INNER JOIN cs.Empresa e " +
                             " WHERE " +
                             "e.Id = :idEmpresa and " +
                             "cs.Data <= :data and " +
                             "cc.Id = :idConta";



                var saldoList = repositorio.SeekByHqlObject(hql, parameters);
                if (saldoList.Any() && saldoList[0][0] != null) decValor += Convert.ToDecimal(saldoList[0][0]);

                return decValor;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public static void RecalcularSaldoEmpresa(int empresaId)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                var parameters = new List<HqlParameter>();
                string where = "";
                if (empresaId > 0)
                {
                    where = " Where el.Empresa.Id = :empresa AND el.Valor <> 0";
                    parameters.Add(new HqlParameter("empresa", empresaId));
                }

                var lancamentosExistentes = repositorio.Buscar<EventoLancamento>(
                    "From EventoLancamento el" + where, parameters);

                foreach (var lancamentosExistente in lancamentosExistentes)
                {
                    AjustarSaldo(repositorio,lancamentosExistente);
                }
            }
        }

        public static void RecalcularSaldoEvento(int eventoId)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                var parameters = new List<HqlParameter>();
                string where = "";
                if (eventoId > 0)
                {
                    where = " Where el.Evento.Id = :evento AND el.Valor <> 0";
                    parameters.Add(new HqlParameter("evento", eventoId));
                }

                var lancamentosExistentes = repositorio.Buscar<EventoLancamento>(
                    "From EventoLancamento el" + where, parameters);

                foreach (var lancamentosExistente in lancamentosExistentes)
                {
                    AjustarSaldo(repositorio, lancamentosExistente);
                }
            }
        }

        public static EventoLancamento LoadNotaExplicativa(int id, int empresaId, DateTime data)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEvento", id));
                    parameters.Add(new HqlParameter("empresa", empresaId));
                    parameters.Add(new HqlParameter("dataEvento", data.Date));
                    const string hql = "FROM EventoLancamento WHERE Evento = :idEvento AND Empresa = :empresa AND Data = :dataEvento ";

                    var evento = repositorio.Buscar<EventoLancamento>(hql, parameters).SingleOrDefault();

                    return evento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static EventoLancamento CarregaEventoHelp(int idEvento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEvento", idEvento));

                    string hql = "SELECT e.Help " +
                                 "FROM EventoLancamento evl " +
                                 "INNER JOIN evl.Evento e " +
                                 "WHERE evl.Evento = :idEvento";

                    var eventohelp = repositorio.SeekByHqlObject(hql, parameters).First()[0].ToString();

                    return new EventoLancamento() {Evento = new Evento() {Help = eventohelp}};


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Evento LoadTipoEvento(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEvento", id));
                    const string hql = "FROM Evento WHERE Id = :idEvento ";

                    var tipoevento = repositorio.Buscar<Evento>(hql, parameters).SingleOrDefault();

                    return tipoevento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        

        public static void AjustarSaldo(IRepositorio repositorio, EventoLancamento eventoLancamento)
        {
            var parameters = new List<HqlParameter>();
            parameters.Add(new HqlParameter("lancamento", eventoLancamento.Id));

            var lancamentosExistentes = repositorio.Buscar<ContaContabilSaldo>(
                "From ContaContabilSaldo ccs Where ccs.EventoLancamento.Id = :lancamento", parameters);

            foreach (var lancamentosExistente in lancamentosExistentes)
            {
                repositorio.Deletar(lancamentosExistente);
            }

            var evento = repositorio.BuscarId<Evento>(eventoLancamento.Evento.Id);

            foreach (var eventoOperacao in evento.ListEventoOperacao)
            {
                ContaContabilSaldo saldo = new ContaContabilSaldo();
                saldo.EventoLancamento = eventoLancamento; 
                saldo.ContaContabil = eventoOperacao.ContaContabil;
                saldo.Empresa = eventoLancamento.Empresa;
                saldo.Data = eventoLancamento.Data;
                if (eventoOperacao.Tipo == EnumEventoOperacaoTipo.Debitar) saldo.Saldo = eventoLancamento.Valor * -1;
                else saldo.Saldo = eventoLancamento.Valor;
                repositorio.Salvar(saldo);
            }
        }

        public static EventoLancamento SalvarOuAtualizarLancamentos(List<EventoLancamento> listEventoLancamento,
            int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    repositorio.Session.BeginTransaction();
                    
                    foreach (var lancamentoGroup in listEventoLancamento.GroupBy(x => x.Data.Date))
                    {
                        var parameters = new List<HqlParameter>();
                        parameters.Add(new HqlParameter("dataEvento", lancamentoGroup.Key));
                        parameters.Add(new HqlParameter("empresaId", idEmpresa));

                        var hqlLancamentosExistes = " FROM EventoLancamento e " +
                                                    " WHERE e.Data = :dataEvento " +
                                                    " AND e.Empresa.Id = :empresaId ";

                        var lancamentosExistens = repositorio.Buscar<EventoLancamento>(hqlLancamentosExistes, parameters);

                        foreach (var lancamento in lancamentoGroup)
                        {
                            LancarEvento(lancamentosExistens, lancamento, repositorio);
                        }

                        decimal saldoGrupo3 = FuncoesContaContabilGrupo.PegaSaldoGrupoDia(idEmpresa, lancamentoGroup.Key);
                        var eventoApuracao = repositorio.Buscar<Evento>("From Evento Where ApuracaoResultado = :resultado",
                            new List<HqlParameter> { new HqlParameter("resultado", EnumSimNao.Sim) }).FirstOrDefault();

                        if (eventoApuracao == null) throw new Exception("Deve ser definido um evento para apuração de resultado");

                        EventoLancamento lancamentoApuracao = new EventoLancamento
                        {
                            Evento = eventoApuracao,
                            Data = lancamentoGroup.Key,
                            Empresa = new Empresa() { Id = idEmpresa },
                            Valor = saldoGrupo3
                        };
                        LancarEvento(lancamentosExistens, lancamentoApuracao, repositorio);
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

        private static void LancarEvento(IList<EventoLancamento> lancamentosExistens, EventoLancamento lancamento, Repositorio.Repositorio repositorio)
        {
            var eventoExistente = lancamentosExistens.FirstOrDefault(x => x.Evento.Id == lancamento.Evento.Id);
            if (eventoExistente != null)
            {
                //if (eventoExistente.Valor != lancamento.Valor)
                //{
                    eventoExistente.Valor = lancamento.Valor;
                    repositorio.Salvar(eventoExistente);
                    AjustarSaldo(repositorio, eventoExistente);
                //}
            }
            else
            {
                repositorio.Salvar(lancamento);
                AjustarSaldo(repositorio, lancamento);
            }
        }

        public static List<EventoLancamento> CarregaValorMensal(List<int> eventosId, int idEmpresa, DateTime dataHoraLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {


                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    var dataPrimeiroDiaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month, 1);
                    var dataUltimaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month,
                        DateTime.DaysInMonth(dataHoraLancamento.Year, dataHoraLancamento.Month));

                    parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMes", dataHoraLancamento));


                    string hql = "SELECT e1.Id, SUM(evl1.Valor) " +
                                 "FROM EventoLancamento evl1 " +
                                 "INNER JOIN evl1.Evento e1 " +
                                 "WHERE evl1.Data BETWEEN :primeiroDiaMes AND :ultimoDiaMes " +
                                 "AND evl1.Empresa.Id = :idEmpresa";

                    Funcoes.HqlParametroIn(eventosId.ToArray(), ref hql, ref parameters, "e1.Id");

                    hql = hql + " GROUP BY e1.Id";

                    var listEventoLancamentoObject = repositorio.SeekByHqlObject(hql, parameters);



                    List<EventoLancamento> listEventoLancamento =
                        listEventoLancamentoObject.Select(evnt => new EventoLancamento
                        {
                            Id = Convert.ToInt32(evnt[0]),
                            ValorMes = Convert.ToDecimal(evnt[1]),
                            Evento = new Evento
                            {
                                Id = Convert.ToInt32(evnt[0]),
                            },

                        }).ToList();

                    return listEventoLancamento;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static Decimal CarregaValorTotalMesAtual(int idEmpresa, DateTime dataHoraLancamento, Enum tipoEvento, int area)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    parameters.Add(new HqlParameter("tipoEvento", tipoEvento));
                    parameters.Add(new HqlParameter("area", area));
                    var dataPrimeiroDiaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month, 1);
                    var dataUltimaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month,DateTime.DaysInMonth(dataHoraLancamento.Year, dataHoraLancamento.Month));

                    parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimaMes));

                    const string hql = "SELECT SUM(evl.Valor) as Valor " +
                                 "FROM EventoLancamento evl " +
                                 "INNER JOIN evl.Evento e1 " +
                                 "WHERE evl.Data BETWEEN :primeiroDiaMes AND :ultimoDiaMes " +
                                 "AND evl.Empresa.Id = :idEmpresa " +
                                 "AND e1.TipoEventoLancamento = :tipoEvento " +
                                 "AND e1.Area = :area";


                    var result = repositorio.SeekByHqlObject(hql, parameters);

                    var valor = Convert.ToDecimal(result[0][0]);

                    return valor;


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Decimal CarregaValorTotalDiaMesAnterior(int idEmpresa, DateTime dataInicial, Enum tipoEvento, int Area)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    parameters.Add(new HqlParameter("tipoEvento", tipoEvento));
                    parameters.Add(new HqlParameter("area", Area));
                    parameters.Add(new HqlParameter("dataInicial", dataInicial));
                    //parameters.Add(new HqlParameter("dataFinal", dataFinal));

                    const string hql = "SELECT SUM(evl.Valor) as Valor " +
                                 "FROM EventoLancamento evl " +
                                 "INNER JOIN evl.Evento e1 " +
                                 "WHERE evl.Data <= :dataInicial " +
                                 "AND evl.Empresa.Id = :idEmpresa " +
                                 "AND e1.TipoEventoLancamento = :tipoEvento " +
                                 "AND e1.Area = :area";


                    var result = repositorio.SeekByHqlObject(hql, parameters);

                    var valor = Convert.ToDecimal(result[0][0]);

                    return valor;


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static EventoLancamento ExisteLancamento(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM EventoLancamento WHERE Empresa = :idEmpresa";

                    var listlancamentos = repositorio.Buscar<EventoLancamento>(hql, parameters).FirstOrDefault();

                    return listlancamentos;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
