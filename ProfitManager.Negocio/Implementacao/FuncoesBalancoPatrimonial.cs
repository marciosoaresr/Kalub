using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesBalancoPatrimonial
    {


        public static List<BalancoPatrimonial> CarregaBalancoPatrimonial(int idEmpresa, DateTime data)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    //var dataPrimeiroDiaMes = new DateTime(2016, 1, 1);
                    var dataUltimoDiaMes = new DateTime(data.Year, data.Month,
                        DateTime.DaysInMonth(data.Year, data.Month));


                    DateTime dataUltimoDiaMesAnterior = dataUltimoDiaMes;
                    dataUltimoDiaMesAnterior = dataUltimoDiaMesAnterior.AddMonths(-1);
                    dataUltimoDiaMesAnterior = new DateTime(dataUltimoDiaMesAnterior.Year, dataUltimoDiaMesAnterior.Month, DateTime.DaysInMonth(dataUltimoDiaMesAnterior.Year, dataUltimoDiaMesAnterior.Month));


                    parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimoDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMesAnterior", dataUltimoDiaMesAnterior));

                    var subSelectSaldoAnterior = " Select sum(ccs.Saldo) " +
                                                 " From ContaContabilSaldo ccs " +
                                                 " Where ccs.ContaContabil = cc.Id and " +
                                                 " ccs.Data <= :ultimoDiaMesAnterior and " +
                                                 " ccs.Empresa = :idEmpresa ";

                    var subSelectSaldoAtual = " Select sum(ccs.Saldo) " +
                                                 " From ContaContabilSaldo ccs " +
                                                 " Where ccs.ContaContabil = cc.Id and " +
                                                 " ccs.Data <= :ultimoDiaMes and " +
                                                 " ccs.Empresa = :idEmpresa ";


                    string hql = " SELECT cc.Codigo, cn.Nome, " +
                                 " ( " + subSelectSaldoAtual + " ) as SaldoAtual, " +
                                 " ( " + subSelectSaldoAnterior + " ) as SaldoAnterior, " +
                                 " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                 " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome, cc.TipoContaContabil " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                 " INNER JOIN ccsg.CampoNome cnsg " +
                                 " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                 " INNER JOIN ccg.CampoNome cng " +
                                 " ORDER BY cc.Codigo ASC ";

                    var listBalancoPatrimonialObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listBalancoPatrimonial =
                        listBalancoPatrimonialObject.Select(balanco =>
                        {
                            var b = new BalancoPatrimonial()
                            {
                                Codigo = Convert.ToString(balanco[0]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(balanco[1])
                                },
                                SaldoAtual = Convert.ToDecimal(balanco[2]),
                                SaldoAnterior = Convert.ToDecimal(balanco[3]),
                                SubGrupoCodigo = Convert.ToString(balanco[4]),
                                SubGrupoNome = Convert.ToString(balanco[5]),
                                GrupoCodigo = Convert.ToString(balanco[6]),
                                GrupoNome = Convert.ToString(balanco[7])

                            };
                            if (((EnumTipoContaContabil?)Convert.ToChar(balanco[8])).GetValueOrDefault(EnumTipoContaContabil.Credora) == EnumTipoContaContabil.Devedora)
                            {
                                b.SaldoAnterior *= -1;
                                b.SaldoAtual *= -1;
                            }
                            return b;
                        }).ToList();

                    return listBalancoPatrimonial;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }



        public static List<BalancoPatrimonial> CarregaPLdiagnostico(int idEmpresa, DateTime data)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    //var dataPrimeiroDiaMes = new DateTime(2016, 1, 1);
                    var dataUltimoDiaMes = new DateTime(data.Year, data.Month,
                        DateTime.DaysInMonth(data.Year, data.Month));


                    DateTime dataUltimoDiaMesAnterior = dataUltimoDiaMes;
                    dataUltimoDiaMesAnterior = dataUltimoDiaMesAnterior.AddMonths(-1);
                    dataUltimoDiaMesAnterior = new DateTime(dataUltimoDiaMesAnterior.Year, dataUltimoDiaMesAnterior.Month, DateTime.DaysInMonth(dataUltimoDiaMesAnterior.Year, dataUltimoDiaMesAnterior.Month));


                    parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimoDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMesAnterior", dataUltimoDiaMesAnterior));

                    var subSelectSaldoMes = " Select sum(ccs.Saldo) " +
                                                 " From ContaContabilSaldo ccs " +
                                                 " Where ccs.ContaContabil = cc.Id and " +
                                                 " ccs.Data <= :ultimoDiaMesAnterior and " +
                                                 " ccs.Empresa = :idEmpresa ";

                    var subSelectSaldoAcumulado = " Select sum(ccs.Saldo) " +
                                                 " From ContaContabilSaldo ccs " +
                                                 " Where ccs.ContaContabil = cc.Id and " +
                                                 " ccs.Data <= :ultimoDiaMes and " +
                                                 " ccs.Empresa = :idEmpresa ";


                    string hql = " SELECT cc.Codigo, cn.Nome, " +
                                 " ( " + subSelectSaldoMes + " ) as SaldoMes, " +
                                 " ( " + subSelectSaldoAcumulado + " ) as SaldoAcumulado, " +
                                 " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                 " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome, cc.TipoContaContabil " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                 " INNER JOIN ccsg.CampoNome cnsg " +
                                 " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                 " INNER JOIN ccg.CampoNome cng " +
                                 " WHERE ccsg.Codigo = '2.03'" +
                                 " ORDER BY cc.Codigo ASC ";

                    var listBalancoPatrimonialObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listBalancoPatrimonial =
                        listBalancoPatrimonialObject.Select(balanco =>
                        {
                            var b = new BalancoPatrimonial()
                            {
                                Codigo = Convert.ToString(balanco[0]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(balanco[1])
                                },
                                SaldoMes = Convert.ToDecimal(balanco[2]),
                                SaldoAcumulado = Convert.ToDecimal(balanco[3]),
                                SubGrupoCodigo = Convert.ToString(balanco[4]),
                                SubGrupoNome = Convert.ToString(balanco[5]),
                                GrupoCodigo = Convert.ToString(balanco[6]),
                                GrupoNome = Convert.ToString(balanco[7])

                            };
                            if (((EnumTipoContaContabil?)Convert.ToChar(balanco[8])).GetValueOrDefault(EnumTipoContaContabil.Credora) == EnumTipoContaContabil.Devedora)
                            {
                                b.SaldoAnterior *= -1;
                                b.SaldoAtual *= -1;
                            }
                            return b;
                        }).ToList();

                    return listBalancoPatrimonial;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
