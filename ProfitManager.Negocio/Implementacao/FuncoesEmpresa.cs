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
    public class FuncoesEmpresa
    {
        public static Empresa Salvar(Empresa empresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(empresa);
                    repositorio.Session.Transaction.Commit();

                    return empresa;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<Empresa> Load(string nomeEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeEmpresa", nomeEmpresa));
                    string hql = " FROM Empresa e " +
                                 " WHERE UPPER(TRIM(e.Nome)) = :nomeEmpresa" +
                                 " OR UPPER(TRIM(e.Nome)) LIKE '%" + nomeEmpresa + "%'";

                    var listEmpresa = repositorio.Buscar<Empresa>(hql, parameters).ToList();

                    return listEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Empresa> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Empresa ";

                    var listEmpresa = repositorio.Buscar<Empresa>(hql, null).ToList();

                    return listEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Empresa Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var Empresa = repositorio.BuscarId<Empresa>(id);

                    return Empresa;
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
                    var empresa = repositorio.BuscarId<Empresa>(id);

                    repositorio.Deletar(empresa);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static string CalculaPeriodoGratis(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var empresaDados = FuncoesEmpresaUsuario.BuscarEmpresa(id);
                    string dataEmpresa = empresaDados.DataHoraCriacao.Value.ToShortDateString();
                    int periodoGratis = Convert.ToInt32(empresaDados.PeriodoGratis);

                    DateTime dataCadastro = Convert.ToDateTime(dataEmpresa);
                    DateTime dataAtual = DateTime.Now;

                    TimeSpan ts = dataAtual - dataCadastro;
                    int differenceInDays = periodoGratis - ts.Days;

                    string dias = Convert.ToString(differenceInDays);

                    return dias;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static string MensagemPeriodoGratis(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    string dias = CalculaPeriodoGratis(id);
                    string msg = null;
                    int dia = Convert.ToInt32(dias);
                    if (dia <= 0)
                    {
                         msg = "Seu período de experiência encerrou, clique aqui e assine o KALUB.";
                    }
                    else
                    {
                         msg = "Seu período de experiência expira em " + dias + " dia(s), clique aqui e assine o KALUB.";
                    }

                    return msg;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static string MensagemDiasUso(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    string dias = CalculaPeriodoGratis(id);
                    string msg = null;
                    int dia = Convert.ToInt32(dias);
                    if (dia <= 0)
                    {
                        msg = "Seu período de uso como Aluno/Professor/Consultor encerrou.";
                    }
                    else
                    {
                        msg = "Seu período de uso como Aluno/Professor/Consultor expira em " + dias + " dia(s).";
                    }

                    return msg;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static string MensagemFaturaVencida(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var faturas = FuncoesEmpresaRecebimento.LoadFaturas(id);
                    string dias = CalculaPeriodoGratis(id);
                    string msg = null;
                    int dia = Convert.ToInt32(dias);
                    if (dia <= 0)
                    {
                        msg = "Sistema bloqueado por falta de pagamento.";
                    }
                    else
                    {
                        msg = "Existe uma cobrança vencida a " + dias + " dia(s).";
                    }

                    return msg;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static List<Empresa> EmailExistente(string email)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("email", email));
                    string hql = " FROM Empresa e " +
                                 " WHERE e.Email = :email";

                    var listEmpresa = repositorio.Buscar<Empresa>(hql, parameters).ToList();

                    return listEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Empresa CnpjExistente(string cnpj)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("cnpj", cnpj));
                    string hql = " FROM Empresa WHERE Cnpj = :cnpj";

                    var listcnpj = repositorio.Buscar<Empresa>(hql, parameters).FirstOrDefault();

                    return listcnpj;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static int CarregaTotalEmpresas(string periodo)
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
                                 "FROM Empresa e " +
                                 "WHERE CONVERT(CHAR(10),e.DataHoraCriacao, 101) BETWEEN :dataInicial AND :dataFinal ";


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

        public static ContaContabilSaldo DeletarContaContabilSaldo(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));

                    var hqlSaldoExistes = " FROM ContaContabilSaldo ccs " +
                                                " WHERE ccs.Empresa.Id = :empresaId ";

                    var saldoExistens = repositorio.Buscar<ContaContabilSaldo>(hqlSaldoExistes, parameters);
                    foreach (var saldos in saldoExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static ContaContabilSaldoInicial DeletarContaContabilSaldoInicial(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM ContaContabilSaldoInicial ccsi" +
                                                " WHERE ccsi.Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<ContaContabilSaldoInicial>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static EventoLancamento DeletarEventoLancamento(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM EventoLancamento evl" +
                                                " WHERE evl.Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<EventoLancamento>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static EmpresaUsuarioLogs DeletarEmpresaUsuarioLog(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM EmpresaUsuarioLogs " +
                                                " WHERE Empresa = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<EmpresaUsuarioLogs>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static EmpresaUsuario DeletarEmpresaUsuario(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM EmpresaUsuario evl" +
                                                " WHERE evl.Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<EmpresaUsuario>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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

        public static Empresa DeletarEmpresa(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM Empresa " +
                                                " WHERE Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<Empresa>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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

        public static FechamentoCaixa DeletarFechamentoCaixa(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM FechamentoCaixa evl" +
                                                " WHERE evl.Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<FechamentoCaixa>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static EmpresaRecebimento DeletarEmpresaRecebimento(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM EmpresaRecebimento " +
                                                " WHERE Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<EmpresaRecebimento>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
        public static EmpresaRecebimentoTransacao DeletarEmpresaRecebimentoTransacao(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    repositorio.Session.BeginTransaction();
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("empresaId", idEmpresa));
                    var hqlSaldoInicialExistes = " FROM EmpresaRecebimentoTransacao " +
                                                " WHERE Empresa.Id = :empresaId ";
                    var saldoinicialExistens = repositorio.Buscar<EmpresaRecebimentoTransacao>(hqlSaldoInicialExistes, parameters);
                    foreach (var saldos in saldoinicialExistens)
                    {
                        repositorio.Deletar(saldos);
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
