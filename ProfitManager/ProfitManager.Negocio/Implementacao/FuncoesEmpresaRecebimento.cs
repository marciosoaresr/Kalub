using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEmpresaRecebimento
    {
        public static EmpresaRecebimento Salvar(EmpresaRecebimento empresaRecebimento)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(empresaRecebimento);
                    repositorio.Session.Transaction.Commit();

                    return empresaRecebimento;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }
        public static List<EmpresaRecebimento> LoadFaturas(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));
                    string hql = " FROM EmpresaTransacaoIugu e " +
                                 " WHERE Empresa = :idEmpresa" +
                                 " ORDER BY DataHoraCriacao DESC ";

                    var listFaturas = repositorio.Buscar<EmpresaRecebimento>(hql, parameters).ToList();

                    return listFaturas;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static EmpresaRecebimento loadTransacao(string idTransacao)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idTransacao", idTransacao));
                    string hql = " FROM EmpresaRecebimento WHERE IdTransacao = :idTransacao";

                    var listtransacao = repositorio.Buscar<EmpresaRecebimento>(hql, parameters).FirstOrDefault();

                    return listtransacao;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static EmpresaRecebimento loadId(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));
                    string hql = " FROM EmpresaRecebimento WHERE Id = :id";

                    var listtransacao = repositorio.Buscar<EmpresaRecebimento>(hql, parameters).FirstOrDefault();

                    return listtransacao;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static Decimal CarregaTotalValorAssinaturas(string periodo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();

                    if (periodo == "hoje")
                    {
                        var dataInicial = DateTime.Today;
                        var dataFinal = DateTime.Today;
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "ontem")
                    {
                        var dataInicial = DateTime.Today.AddDays(-1);
                        var dataFinal = DateTime.Today.AddDays(-1);
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "sete-dias")
                    {
                        var dataFinal = DateTime.Today;
                        var dataInicial = DateTime.Today.AddDays(-7);
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "mes-atual")
                    {
                        var dataInicial = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        var dataFinal = DateTime.Today;
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "mes-anterior")
                    {
                        DateTime dataMesAnterior = DateTime.Today;
                        dataMesAnterior = dataMesAnterior.AddMonths(-1);
                        var dataInicial = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, 1);
                        var dataFinal = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "todos")
                    {
                        var dataInicial = new DateTime(2016, 1, 1);
                        var dataFinal = DateTime.Today;

                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    const string hql = "SELECT SUM(er.Valor) as Valor " +
                                 "FROM EmpresaTransacaoIugu er " +
                                 "WHERE CONVERT(CHAR(10),er.DataVencimento, 101) BETWEEN :dataInicial AND :dataFinal " +
                                 "AND er.Status = 'paid' ";

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

        public static int CarregaTotalAssinaturas(string periodo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();

                    if (periodo == "hoje")
                    {
                        var dataInicial = DateTime.Today;
                        var dataFinal = DateTime.Today;
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "ontem")
                    {
                        var dataInicial = DateTime.Today.AddDays(-1);
                        var dataFinal = DateTime.Today.AddDays(-1);
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "sete-dias")
                    {
                        var dataFinal = DateTime.Today;
                        var dataInicial = DateTime.Today.AddDays(-7);
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "mes-atual")
                    {
                        var dataInicial = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        var dataFinal = DateTime.Today;
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "mes-anterior")
                    {
                        DateTime dataMesAnterior = DateTime.Today;
                        dataMesAnterior = dataMesAnterior.AddMonths(-1);
                        var dataInicial = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, 1);
                        var dataFinal = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    if (periodo == "todos")
                    {
                        var dataInicial = new DateTime(2016, 1, 1);
                        var dataFinal = DateTime.Today;

                        parameters.Add(new HqlParameter("dataInicial", dataInicial));
                        parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    }
                    const string hql = "SELECT COUNT(*) " +
                                 "FROM EmpresaTransacaoIugu " +
                                 "WHERE CONVERT(CHAR(10),DataVencimento, 101) BETWEEN :dataInicial AND :dataFinal " +
                                 "AND Status = 'paid' ";


                    var result = repositorio.SeekByHqlObject(hql, parameters);

                    var valor = Convert.ToInt32(result[0][0]);

                    return valor;


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
