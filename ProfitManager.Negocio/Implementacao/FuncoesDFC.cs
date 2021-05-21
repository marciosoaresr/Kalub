using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesDfc
    {

        private static List<List<object>> GetDadosDfc(IRepositorio repositorio, int idEmpresa, DateTime dataInicial, DateTime dataFinal)
        {
            var parameters = new List<HqlParameter>();
            parameters.Add(new HqlParameter("idEmpresa", idEmpresa));


            parameters.Add(new HqlParameter("dataInicial", dataInicial));
            parameters.Add(new HqlParameter("dataFinal", dataFinal));

            string hql = " SELECT e.Codigo, ecampo.Nome, sum(coalesce(evl.Valor,0)), a.Id, e.TipoDFC, cco.TipoContaContabil " +
                         " FROM ContaContabilSaldo cs " +
                         " INNER JOIN cs.EventoLancamento evl " +
                         " INNER JOIN evl.Evento e " +
                         " INNER JOIN e.CampoNome ecampo " +
                         " INNER JOIN e.Area a " +
                         " INNER JOIN cs.ContaContabil cco " +
                         " WHERE cs.Empresa = :idEmpresa " +
                         " and cs.Data >= :dataInicial " +
                         " and cs.Data <= :dataFinal " +
                         " and cco.Codigo = '1.01.01' " +
                         " GROUP BY e.Codigo, ecampo.Nome, a.Id, e.TipoDFC,cco.TipoContaContabil ";
            return repositorio.SeekByHqlObject(hql, parameters);
        } 

        public static List<DemonstracaoFluxoCaixa> CarregaDfc(int idEmpresa, DateTime data)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var dataInicial = new DateTime(data.Year, data.Month, 1);
                    var dataFinal = new DateTime(data.Year, data.Month,
                        DateTime.DaysInMonth(data.Year, data.Month));

                    var listDfcObject = GetDadosDfc(repositorio, idEmpresa, dataInicial, dataFinal);

                    var listDfc =
                        listDfcObject.Select(dfc =>
                        {
                            var d = new DemonstracaoFluxoCaixa()
                            {
                                Codigo = Convert.ToString(dfc[0]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(dfc[1])
                                },
                                Saldo =  Convert.ToInt32(dfc[2]),
                                Area = Convert.ToInt32(dfc[3]),
                                TipoDfc = Convert.ToString(dfc[4]),
                                SaldoAcumulado = 0
                            };
                            if ((EnumTipoContaContabil)Convert.ToChar(dfc[4]) == EnumTipoContaContabil.Devedora)
                                d.Saldo *= -1;
                            return d;
                        }).ToList();

                    dataInicial = new DateTime(data.Year, 1, 1);
                    dataFinal = new DateTime(data.Year, data.Month,
                        DateTime.DaysInMonth(data.Year, data.Month));

                    listDfcObject = GetDadosDfc(repositorio, idEmpresa, dataInicial, dataFinal);

                    foreach (var dfc in listDfcObject)
                    {
                        var itemDfc = listDfc.FirstOrDefault(x => x.Codigo == Convert.ToString(dfc[0]));
                        if (itemDfc == null)
                        {
                            itemDfc = new DemonstracaoFluxoCaixa()
                            {
                                Codigo = Convert.ToString(dfc[0]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(dfc[1])
                                },
                                SaldoAcumulado = Convert.ToInt32(dfc[2]),
                                Area = Convert.ToInt32(dfc[3]),
                                TipoDfc = Convert.ToString(dfc[4])
                            };
                            if ((EnumTipoContaContabil)Convert.ToChar(dfc[4]) == EnumTipoContaContabil.Devedora)
                                itemDfc.Saldo *= -1;
                            listDfc.Add(itemDfc);
                        }
                        else
                        {
                            decimal valorAdd = Convert.ToInt32(dfc[2]);
                            if ((EnumTipoContaContabil)Convert.ToChar(dfc[4]) == EnumTipoContaContabil.Devedora)
                                valorAdd *= -1;
                            itemDfc.SaldoAcumulado += valorAdd;
                        }
                    }

                    return listDfc;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
