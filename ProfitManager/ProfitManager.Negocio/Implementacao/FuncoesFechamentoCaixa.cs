
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesFechamentoCaixa
    {
        public static FechamentoCaixa Salvar(FechamentoCaixa eventoLancamento)
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

        public static List<FechamentoCaixa> Salvar(List<FechamentoCaixa> listEventoLancamento)
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

       public static List<FechamentoCaixa> LoadByDocumentoAndEmpresaAndData(List<TipoDocumentoFechamentoCaixa>  tipoDocumento, int idEmpresa, DateTime dataHoraLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                
                    try
                    {
                        var fechamentos = new List<FechamentoCaixa>();

                        var fechamentosDiarios = BuscarFechamentos(repositorio, dataHoraLancamento, idEmpresa,
                            tipoDocumento.Select(x => x.Id).ToList());


                        fechamentos.AddRange(fechamentosDiarios);


                        return fechamentos;
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                
            }
        }

        public static List<FechamentoCaixa> BuscarFechamentos(IRepositorio repositorio, DateTime dataHoraLancamento,
     int idEmpresa, List<int> fechamentosId)
        {
            var parameters = new List<HqlParameter>();
            parameters.Add(new HqlParameter("dataLancamento", dataHoraLancamento.Date));
            parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
           

           

            //                       0        1          2               3            4         5
            string hql = " SELECT evl.Id, evl.Valor, evl.Data, evl.DataHoraCriacao, e.Id,  e.Codigo, ecampo.Nome, emp.Id " +
                         " FROM FechamentoCaixa evl " +
                         " INNER JOIN evl.TipoDocumentoFechamentoCaixa e " +
                         " INNER JOIN e.CampoNome ecampo " +
                         " INNER JOIN evl.Empresa emp " +
                         " WHERE emp.Id = :idEmpresa " +
                         " AND evl.Data = :dataLancamento ";

            Funcoes.HqlParametroIn(fechamentosId.ToArray(), ref hql, ref parameters, "e.Id");
            hql += "ORDER BY e.Codigo ";
            var listEventoLancamentoObject = repositorio.SeekByHqlObject(hql, parameters);


            List<FechamentoCaixa> listFechamentoCaixa =
                listEventoLancamentoObject.Select(evnt => new FechamentoCaixa()
                {
                    Id = Convert.ToInt32(evnt[0]),
                    Valor = Convert.ToDecimal(evnt[1]),
                    Data = Convert.ToDateTime(evnt[2]),
                    DataHoraCriacao = Convert.ToDateTime(evnt[3]),
                    TipoDocumentoFechamentoCaixa = new TipoDocumentoFechamentoCaixa()
                    {
                        Id = Convert.ToInt32(evnt[4]),
                        Codigo = Convert.ToString(evnt[5]),
                        CampoNome = new Campo
                        {
                            Nome = Convert.ToString(evnt[6])
                        },
                    },
                    Empresa = new Empresa
                    {
                        Id = Convert.ToInt32(evnt[7])
                    }

                }).ToList();

            return listFechamentoCaixa;
        }


       

        public static FechamentoCaixa SalvarOuAtualizarFechamentos(List<FechamentoCaixa> listFechamentoCaixa,
            DateTime dataHoraLancamento, int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    repositorio.Session.BeginTransaction();

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("dataFechamento", dataHoraLancamento.Date));
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));

                    var hqlFechamentosExistes = " FROM FechamentoCaixa e " +
                                                " WHERE e.Data = :dataFechamento " +
                                                " AND e.Empresa.Id = :empresaId ";

                    Funcoes.HqlParametroIn(listFechamentoCaixa.Select(x => x.TipoDocumentoFechamentoCaixa.Id).ToArray(),
                        ref hqlFechamentosExistes, ref parameters, "e.TipoDocumentoFechamentoCaixa.Id");

                    var fechamentosExistens = repositorio.Buscar<FechamentoCaixa>(hqlFechamentosExistes, parameters);


                    foreach (var lancamento in listFechamentoCaixa)
                    {

                        var fechamentoExistente = fechamentosExistens.FirstOrDefault(x => x.TipoDocumentoFechamentoCaixa.Id == lancamento.TipoDocumentoFechamentoCaixa.Id);
                        if (fechamentoExistente != null)
                        {
                            if (fechamentoExistente.Valor != lancamento.Valor)
                            {
                                fechamentoExistente.Valor = lancamento.Valor;
                                repositorio.Salvar(fechamentoExistente);
                            }
                        }
                        else
                        {
                            if (lancamento.Valor != 0)
                            {
                                repositorio.Salvar(lancamento);
                            }
                        }

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

        
        public static List<FechamentoCaixa> CarregaValorDiario(List<int> fechamentosId, int idEmpresa, DateTime dataHoraLancamento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {


                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    parameters.Add(new HqlParameter("dataLancamento", dataHoraLancamento.Date));

                    //var dataPrimeiroDiaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month, 1);
                    //var dataUltimaMes = new DateTime(dataHoraLancamento.Year, dataHoraLancamento.Month,
                    //DateTime.DaysInMonth(dataHoraLancamento.Year, dataHoraLancamento.Month));

                    //parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
                    //parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimaMes));


                    string hql = "SELECT e1.Id, SUM(evl1.Valor) " +
                                 "FROM FechamentoCaixa evl1 " +
                                 "INNER JOIN evl1.TipoDocumentoFechamentoCaixa e1 " +
                                 "WHERE evl1.Data = :dataLancamento " +
                                 "AND evl1.Empresa.Id = :idEmpresa";

                    Funcoes.HqlParametroIn(fechamentosId.ToArray(), ref hql, ref parameters, "e1.Id");

                    hql = hql + " GROUP BY e1.Id";

                    var listEventoLancamentoObject = repositorio.SeekByHqlObject(hql, parameters);



                    List<FechamentoCaixa> listFechamentoCaixa =
                        listEventoLancamentoObject.Select(evnt => new FechamentoCaixa()
                        {
                            Id = Convert.ToInt32(evnt[0]),
                            Valor = Convert.ToDecimal(evnt[1]),
                            TipoDocumentoFechamentoCaixa = new TipoDocumentoFechamentoCaixa()
                            {
                                Id = Convert.ToInt32(evnt[0]),
                            },

                        }).ToList();

                    return listFechamentoCaixa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
