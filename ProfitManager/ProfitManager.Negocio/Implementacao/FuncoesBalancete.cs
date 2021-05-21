using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesBalancete
    {


        public static List<Balancete> CarregaBalancete(int idEmpresa, DateTime dataInicial)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var dataInicio = new DateTime(dataInicial.Year, dataInicial.Month, 1);
                    var dataFim = new DateTime(dataInicial.Year, dataInicial.Month,
                        DateTime.DaysInMonth(dataInicial.Year, dataInicial.Month));

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("dataInicial", dataInicio));
                    parameters.Add(new HqlParameter("dataFinal", dataFim));
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    var subSelectSaldoAnterior = " Select sum(ccs.Saldo) " +
                                                 " From ContaContabilSaldo ccs " +
                                                 " Where ccs.ContaContabil = cc.Id and " +
                                                 " ccs.Data < :dataInicial and " +
                                                 " ccs.Empresa = :idEmpresa ";

                    var subSelectDebito = " Select sum(ccs.Saldo) " +
                                          " From ContaContabilSaldo ccs " +
                                          " Where ccs.ContaContabil = cc.Id and " +
                                          " ccs.Data >= :dataInicial and " +
                                          " ccs.Data <= :dataFinal and " +
                                          " ccs.Saldo < 0 and " +
                                          " ccs.Empresa = :idEmpresa ";

                    var subSelectCredito = " Select sum(ccs.Saldo) " +
                                           " From ContaContabilSaldo ccs " +
                                           " Where ccs.ContaContabil = cc.Id and " +
                                           " ccs.Data >= :dataInicial and " +
                                           " ccs.Data <= :dataFinal and " +
                                           " ccs.Saldo > 0 and " +
                                           " ccs.Empresa = :idEmpresa ";

                    string hql = " SELECT cc.Codigo, cn.Nome, " +
                                 " ( " + subSelectSaldoAnterior + " ) as SaldoAnterior, " +
                                 " ( " + subSelectDebito + " ) as Debito, " +
                                 " ( " + subSelectCredito + " ) as Credito, " +
                                 " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                 " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                 " INNER JOIN ccsg.CampoNome cnsg " +
                                 " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                 " INNER JOIN ccg.CampoNome cng " +
                                 " ORDER BY cc.Codigo ASC ";

                    var listBalanceteObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listBalancete =
                        listBalanceteObject.Select(balancete => new Balancete()
                        {
                            Codigo = Convert.ToString(balancete[0]),
                            CampoNome = new Campo
                            {
                                Nome = Convert.ToString(balancete[1])
                            },
                            SaldoAnterior = Convert.ToDecimal(balancete[2]),
                            Debito = Convert.ToDecimal(balancete[3]),
                            Credito = Convert.ToDecimal(balancete[4]),
                            SubGrupoCodigo = Convert.ToString(balancete[5]),
                            SubGrupoNome = Convert.ToString(balancete[6]),
                            GrupoCodigo = Convert.ToString(balancete[7]),
                            GrupoNome = Convert.ToString(balancete[8])

                        }).ToList();

                    return listBalancete;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Balancete> CarregaSaldosIniciais(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    var subSelectCredito = " Select sum(ccs.Saldo) " +
                                           " From ContaContabilSaldo ccs " +
                                           " Where ccs.ContaContabil = cc.Id and " +
                                           " ccs.Saldo > 0 and " +
                                           " cc.ExigeSaldoinicial = 'S' and " +
                                           " ccs.Empresa = :idEmpresa ";

                    string hql = " SELECT cc.Codigo, cc.ExigeSaldoinicial, cn.Nome, " +
                                 " ( " + subSelectCredito + " ) as Credito, " +
                                 " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                 " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                 " INNER JOIN ccsg.CampoNome cnsg " +
                                 " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                 " INNER JOIN ccg.CampoNome cng " +
                                 " ORDER BY cc.Codigo ASC ";

                    var listBalanceteObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listBalancete =
                        listBalanceteObject.Select(balancete => new Balancete()
                        {
                            Codigo = Convert.ToString(balancete[0]),
                            ExigeSaldoinicial = (EnumExigeSaldoinicial)Convert.ToChar(balancete[1]),
                            CampoNome = new Campo
                            {
                                Nome = Convert.ToString(balancete[2])
                            },
                            Credito = Convert.ToDecimal(balancete[3]),
                            SubGrupoCodigo = Convert.ToString(balancete[4]),
                            SubGrupoNome = Convert.ToString(balancete[5]),
                            GrupoCodigo = Convert.ToString(balancete[6]),
                            GrupoNome = Convert.ToString(balancete[7])

                        }).ToList();

                    return listBalancete;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
