using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.EntidadeAuxiliar;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesGrupoDre
    {
        public static List<GrupoDRE> Load(string nomeGrupoDre)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("nomeGrupoDRE", nomeGrupoDre));
                    string hql = " FROM GrupoDRE e " +
                                 " WHERE UPPER(TRIM(Nome)) = :nomeGrupoDRE" +
                                 " OR  UPPER(TRIM(Nome)) LIKE '%" + nomeGrupoDre + "%'";

                    var listGrupoDre = repositorio.Buscar<GrupoDRE>(hql, parameters).ToList();

                    return listGrupoDre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<GrupoDRE> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM GrupoDRE ";

                    var listGrupoDre = repositorio.Buscar<GrupoDRE>(hql, null).ToList();

                    return listGrupoDre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static GrupoDRE Salvar(GrupoDRE grupodre)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(grupodre);
                    repositorio.Session.Transaction.Commit();

                    return grupodre;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static GrupoDRE Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var grupodre = repositorio.BuscarId<GrupoDRE>(id);

                    return grupodre;
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
                    var grupodre = repositorio.BuscarId<GrupoDRE>(id);

                    repositorio.Deletar(grupodre);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static DreAuxiliar CarregaDre(int idEmpresa, DateTime dataInicial)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    DreAuxiliar dre = new DreAuxiliar();

                    var selectGrupo = repositorio.SeekByHqlObject("Select Codigo, Nome, Totalizador from GrupoDRE Order by Codigo", null);

                    foreach (var item in selectGrupo)
                    {
                        DreGrupoAuxiliar grupo = new DreGrupoAuxiliar()
                        {
                            Codigo = (string)item[0],
                            Nome = item[1].ToString(),
                            Totalizador = (EnumSimNao) Convert.ToChar(item[2]) == EnumSimNao.Sim,
                            DreAuxiliar = dre
                        };
                        dre.DreGrupoAuxiliarList.Add(grupo);
                    }



                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    var dataPrimeiroDiaMes = new DateTime(dataInicial.Year, dataInicial.Month, 1);
                    var dataUltimoDiaMes = new DateTime(dataInicial.Year, dataInicial.Month,
                        DateTime.DaysInMonth(dataInicial.Year, dataInicial.Month));

                    DateTime dataPrimeiroDiaMesAnterior = dataPrimeiroDiaMes;
                    dataPrimeiroDiaMesAnterior = dataPrimeiroDiaMesAnterior.AddMonths(-1);
                    DateTime dataUltimoDiaMesAnterior = new DateTime(dataPrimeiroDiaMesAnterior.Year, dataPrimeiroDiaMesAnterior.Month, 1).AddMonths(1).AddDays(-1);


                    parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimoDiaMes));

                    parameters.Add(new HqlParameter("primeiroDiaMesAnterior", dataPrimeiroDiaMesAnterior));
                    parameters.Add(new HqlParameter("ultimoDiaMesAnterior", dataUltimoDiaMesAnterior));


                    var dataPrimeiroDiaAno = new DateTime(dataInicial.Year, 1, 1);
                    parameters.Add(new HqlParameter("primeiroDiaAno", dataPrimeiroDiaAno));
                     
                    var subSelectSaldo = " Select sum(ccs.Saldo) " +
                                           " From " +
                                           " ContaContabilSaldo ccs " +
                                           " Where ccs.ContaContabil = cc.Id and " +
                                           " ccs.Data >= :primeiroDiaMes and " +
                                           " ccs.Data <= :ultimoDiaMes and " +
                                           " ccs.Empresa = :idEmpresa ";

                    var subSelectSaldoMesAnterior = " Select sum(ccs.Saldo) " +
                                                    " From " +
                                                    " ContaContabilSaldo ccs " +
                                                    " Where ccs.ContaContabil = cc.Id and " +
                                                    " ccs.Data >= :primeiroDiaMesAnterior and " +
                                                    " ccs.Data <= :ultimoDiaMesAnterior and " +
                                                    " ccs.Empresa = :idEmpresa ";

                    var subSelectSaldoAcumulado = " Select sum(ccs.Saldo) " +
                                                  " From " +
                                                  " ContaContabilSaldo ccs " +
                                                  " Where ccs.ContaContabil = cc.Id and " +
                                                  " ccs.Data >= :primeiroDiaAno and " +
                                                  " ccs.Data <= :ultimoDiaMes and " +
                                                  " ccs.Empresa = :idEmpresa ";

                    string hql = " SELECT dre.Codigo, cn.Nome, " +
                                 " ( " + subSelectSaldo + " ) as Saldo, " +
                                 " ( " + subSelectSaldoAcumulado + " ) as SaldoAcumulado, " +
                                 " ( " + subSelectSaldoMesAnterior + " ) as SaldoMesAnterior " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.GrupoDRE dre " +
                                 " ORDER BY dre.Codigo ASC ";

                    var listDreObject = repositorio.SeekByHqlObject(hql, parameters);

                    foreach (var grupoItem in listDreObject.GroupBy(x => x[0]))
                    {
                        var grupo = dre.DreGrupoAuxiliarList.FirstOrDefault(x => x.Codigo == (string) grupoItem.Key);
                        foreach (var conta in grupoItem.OrderBy(x => x[1].ToString()))
                        {
                            DreGrupoItemAuxiliar itemAuxiliar = new DreGrupoItemAuxiliar()
                            {
                                Codigo = conta[0].ToString(),
                                Nome = conta[1].ToString(),
                                Valor = Convert.ToDecimal(conta[2]),
                                ValorAcumulado = Convert.ToDecimal(conta[3]),
                                ValorMesAnterior = Convert.ToDecimal(conta[4])
                            };
                            // Exibir somente contas com saldo
                            if (itemAuxiliar.ValorAcumulado != 0)
                            {
                                grupo.DreGrupoItemAuxiliarList.Add(itemAuxiliar);
                            }
                        }
                    }
                    return dre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static DreAuxiliar CarregaPrevisaoDre(int idEmpresa, DateTime dataInicial)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    DreAuxiliar dre = new DreAuxiliar();

                    var selectGrupo = repositorio.SeekByHqlObject("Select Codigo, Nome, Totalizador from GrupoDRE Order by Codigo", null);

                    foreach (var item in selectGrupo)
                    {
                        DreGrupoAuxiliar grupo = new DreGrupoAuxiliar()
                        {
                            Codigo = (string)item[0],
                            Nome = item[1].ToString(),
                            Totalizador = (EnumSimNao)Convert.ToChar(item[2]) == EnumSimNao.Sim,
                            DreAuxiliar = dre
                        };
                        dre.DreGrupoAuxiliarList.Add(grupo);
                    }

                    var parameters = new List<HqlParameter>();

                    string hql = " SELECT dre.Codigo, cn.Nome " +
                                 " From " +
                                 " ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome cn " +
                                 " INNER JOIN cc.GrupoDRE dre " +
                                 " ORDER BY dre.Codigo ASC ";

                    var listDreObject = repositorio.SeekByHqlObject(hql, parameters);

                    foreach (var grupoItem in listDreObject.GroupBy(x => x[0]))
                    {
                        var grupo = dre.DreGrupoAuxiliarList.FirstOrDefault(x => x.Codigo == (string)grupoItem.Key);
                        foreach (var conta in grupoItem.OrderBy(x => x[1].ToString()))
                        {
                            DreGrupoItemAuxiliar itemAuxiliar = new DreGrupoItemAuxiliar()
                            {
                                Codigo = conta[0].ToString(),
                                Nome = conta[1].ToString()
                            };
                            // Exibir somente contas com saldo
                            //if (itemAuxiliar.ValorAcumulado != 0)
                            //{
                               grupo.DreGrupoItemAuxiliarList.Add(itemAuxiliar);
                            //}
                        }
                    }
                    return dre;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }
}
