using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesExtratoConta
    {


        public static List<ExtratoConta> CarregaExtrato (int idEmpresa, DateTime dataInicial, int idConta)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idConta", idConta));
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    var dataPrimeiroDiaMes = new DateTime(dataInicial.Year, dataInicial.Month, 1);
                    var dataUltimaMes = new DateTime(dataInicial.Year, dataInicial.Month,
                        DateTime.DaysInMonth(dataInicial.Year, dataInicial.Month));

                    parameters.Add(new HqlParameter("primeiroDiaMes", dataPrimeiroDiaMes));
                    parameters.Add(new HqlParameter("ultimoDiaMes", dataUltimaMes));

                    string hql = " SELECT e.Codigo, ecampo.Nome, sum(cs.Saldo), a.Id, cco.TipoContaContabil " +
                                 " FROM ContaContabilSaldo cs " +
                                 " INNER JOIN cs.EventoLancamento evl " +
                                 " INNER JOIN evl.Evento e " +
                                 " INNER JOIN e.CampoNome ecampo " +
                                 " INNER JOIN e.Area a " +
                                 " INNER JOIN cs.ContaContabil cco " +
                                 " WHERE cco.Id = :idConta " +
                                 " and cs.Empresa = :idEmpresa " +
                                 " and cs.Data >= :primeiroDiaMes " +
                                 " and cs.Data <= :ultimoDiaMes " +
                                 " GROUP BY e.Codigo, ecampo.Nome, a.Id, cco.TipoContaContabil ";

                    var listExtratoObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listExtrato =
                        listExtratoObject.Select(extrato =>
                        {
                            var e = new ExtratoConta()
                            {
                                Codigo = Convert.ToString(extrato[0]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(extrato[1])
                                },
                                Saldo = Convert.ToDecimal(extrato[2]),
                                Area = Convert.ToInt32(extrato[3])
                            };
                            if ((EnumTipoContaContabil) Convert.ToChar(extrato[4]) == EnumTipoContaContabil.Devedora)
                                e.Saldo = e.Saldo*-1;
                            return e;
                        }).ToList();

                    return listExtrato;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<ExtratoConta> ListContaContabil(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    string hql = " SELECT cc.Codigo, cc.Id, ecampo.Nome " +
                                 " FROM ContaContabilSaldo cs " +
                                 " INNER JOIN cs.ContaContabil cc " +
                                 " INNER JOIN cc.CampoNome ecampo " +
                                 " WHERE cs.Empresa = :idEmpresa " +
                                 " and cs.Saldo <> 0 " +
                                 " and cs.Data <> '1800-01-01' " +
                                 " GROUP BY cc.Codigo, cc.Id, ecampo.Nome ";




                    var listContaContabilObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listContaContabil =
                        listContaContabilObject.Select(extrato => new ExtratoConta()
                        {
                            Id = Convert.ToInt32(extrato[1]),
                            Codigo = Convert.ToString(extrato[0]),
                            CampoNome = new Campo
                            {
                                Nome = Convert.ToString(extrato[2])
                            }
                        }).ToList();

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
