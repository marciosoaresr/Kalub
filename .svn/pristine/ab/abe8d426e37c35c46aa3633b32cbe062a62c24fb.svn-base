using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesContaContabilSaldoInicial
    {
        public static List<ContaContabilSaldoInicial> LoadByEmpresa(int idEmpresa, bool somenteExigeSaldoInicial)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    //                       0             1                 2      3       4       5               6
                    string hql = " SELECT csi.Id, csi.DataHoraCriacao, csi.Valor, cc.Id, cn.Nome, e.Id, cc.NotaExplicativaContaContabil, cc.ExigeSaldoinicial, " +
                                       " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                       " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome, ccsg.NotaExplicativaSubGrupo, ccg.NotaExplicativaGrupo, ecampohelp.Nome, cc.LucroPrejuizoAcumulado, cc.TipoContaContabil, cc.Help " +
                                       " FROM ContaContabilSaldoInicial csi " +
                                       " INNER JOIN csi.ContaContabil cc " +
                                       " INNER JOIN cc.CampoNome cn " +
                                       " INNER JOIN cc.CampoHelp ecampohelp " +
                                       " INNER JOIN cc.ContaContabilSubGrupo ccsg " +
                                       " INNER JOIN ccsg.CampoNome cnsg " +
                                       " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                       " INNER JOIN ccg.CampoNome cng " +
                                       " INNER JOIN csi.Empresa e " +
                                       " WHERE e.Id = :idEmpresa ";


                    if (somenteExigeSaldoInicial)
                    {
                        hql += " and (cc.ExigeSaldoinicial = :sim " +
                               " or cc.LucroPrejuizoAcumulado = :sim) ";
                        parameters.Add(new HqlParameter("sim", EnumSimNao.Sim));
                        parameters.Add(new HqlParameter("sim", EnumSimNao.Sim));
                    }

                    var listContaContabilObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listContaContabilSaldoInicial =
                        listContaContabilObject.Select(ccSaldoInicial => new ContaContabilSaldoInicial()
                        {
                            Id = Convert.ToInt32(ccSaldoInicial[0]),
                            DataHoraCriacao = Convert.ToDateTime(ccSaldoInicial[1]),
                            Valor = Convert.ToDecimal(ccSaldoInicial[2]),
                            ContaContabil = new ContaContabil
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[3]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(ccSaldoInicial[4])
                                }
                            },
                            Empresa = new Empresa
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[5])
                            },
                            NotaExplicativaContaContabil = Convert.ToString(ccSaldoInicial[6]),
                            ExigeSaldoinicial = (EnumExigeSaldoinicial)Convert.ToChar(ccSaldoInicial[7]),
                            SubGrupoCodigo = Convert.ToString(ccSaldoInicial[8]),
                            SubGrupoNome = Convert.ToString(ccSaldoInicial[9]),
                            GrupoCodigo = Convert.ToString(ccSaldoInicial[10]),
                            GrupoNome = Convert.ToString(ccSaldoInicial[11]),
                            NotaExplicativaGrupo = Convert.ToString(ccSaldoInicial[12]),
                            NotaExplicativaSubGrupo = Convert.ToString(ccSaldoInicial[13]),
                            CampoHelp = new Campo
                            {
                                Nome = Convert.ToString(ccSaldoInicial[14])
                            },
                            LucroPrejuizoAcumulado = (EnumSimNao)Convert.ToChar(ccSaldoInicial[15]),
                            TipoContaContabil = ccSaldoInicial[16] == null ? EnumTipoContaContabil.Credora : (EnumTipoContaContabil)Convert.ToChar(ccSaldoInicial[16]),
                            Help = Convert.ToString(ccSaldoInicial[17])


                        }).ToList();

                    return listContaContabilSaldoInicial;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<ContaContabilSaldoInicial> LoadById(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("id", id));

                    //                       0             1                 2      3       4       5               6
                    string hql = " SELECT csi.Id, csi.DataHoraCriacao, csi.Valor, cc.Id, cn.Nome, e.Id, cc.NotaExplicativaContaContabil, cc.Help " +
                                       " FROM ContaContabilSaldoInicial csi " +
                                       " INNER JOIN csi.ContaContabil cc " +
                                       " INNER JOIN cc.CampoNome cn " +
                                       " INNER JOIN csi.Empresa e " +
                                       " WHERE  csi.Id = :id ";

                    var listContaContabilObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listContaContabilSaldoInicial =
                        listContaContabilObject.Select(ccSaldoInicial => new ContaContabilSaldoInicial()
                        {
                            Id = Convert.ToInt32(ccSaldoInicial[0]),
                            DataHoraCriacao = Convert.ToDateTime(ccSaldoInicial[1]),
                            Valor = Convert.ToDecimal(ccSaldoInicial[2]),
                            ContaContabil = new ContaContabil
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[3]),
                                Help = Convert.ToString(ccSaldoInicial[7]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(ccSaldoInicial[4])
                                }
                            },
                            Empresa = new Empresa
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[5])
                            },
                            NotaExplicativaContaContabil = Convert.ToString(ccSaldoInicial[6])

                        }).ToList();

                    return listContaContabilSaldoInicial;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        public static List<ContaContabilSaldoInicial> Load(List<int> listId)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();

                    //                      0               1               2       3       4      5
                    string hql = " SELECT csi.Id, csi.DataHoraCriacao, csi.Valor, cc.Id, cn.Nome, e.Id   " +
                                      " FROM ContaContabilSaldoInicial csi " +
                                      " INNER JOIN csi.ContaContabil cc " +
                                      " INNER JOIN cc.CampoNome cn " +
                                      " INNER JOIN csi.Empresa e " +
                                      " WHERE 1 = 1 ";

                    Funcoes.HqlParametroIn(listId.ToArray(), ref hql, ref parameters, "csi.Id");

                    var listContaContabilObject = repositorio.SeekByHqlObject(hql, parameters);

                    var listContaContabilSaldoInicial =
                        listContaContabilObject.Select(ccSaldoInicial => new ContaContabilSaldoInicial()
                        {
                            Id = Convert.ToInt32(ccSaldoInicial[0]),
                            DataHoraCriacao = Convert.ToDateTime(ccSaldoInicial[1]),
                            Valor = Convert.ToDecimal(ccSaldoInicial[2]),
                            ContaContabil = new ContaContabil
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[3]),
                                CampoNome = new Campo
                                {
                                    Nome = Convert.ToString(ccSaldoInicial[4])
                                }
                            },
                            Empresa = new Empresa
                            {
                                Id = Convert.ToInt32(ccSaldoInicial[5])
                            }
                        }).ToList();

                    return listContaContabilSaldoInicial;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<ContaContabilSaldoInicial> Salvar(
            List<ContaContabilSaldoInicial> listContaContabilSaldoInicial)
        {
            if (!listContaContabilSaldoInicial.Any())
                return listContaContabilSaldoInicial;

            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    //Preencher dados conta contabil
                    var contaContabilList = repositorio.SeekByHqlObject("Select Id, Codigo, LucroPrejuizoAcumulado, TipoContaContabil From ContaContabil Where Id in (:ids)",
                       new List<HqlParameter>()
                       {
                            new HqlParameter("ids", listContaContabilSaldoInicial.Select(x => x.ContaContabil.Id))
                       });

                    foreach (var contaContabilSaldoInicial in listContaContabilSaldoInicial)
                    {
                        var item =
                            contaContabilList.FirstOrDefault(
                                x => Convert.ToInt32(x[0]) == contaContabilSaldoInicial.ContaContabil.Id);

                        if (item != null)
                        {
                            contaContabilSaldoInicial.ContaContabil.Codigo = item[1].ToString();
                            contaContabilSaldoInicial.ContaContabil.LucroPrejuizoAcumulado = (EnumSimNao)Convert.ToChar(item[2]);
                            contaContabilSaldoInicial.ContaContabil.TipoContaContabil = item[3] == null
                                ? EnumTipoContaContabil.Credora
                                : (EnumTipoContaContabil)Convert.ToChar(item[3]);
                        }
                    }

                    //Salvar lancamentos
                    var empresa = listContaContabilSaldoInicial.FirstOrDefault().Empresa;
                    var saldosList = LoadByEmpresa(empresa.Id, false);
                    foreach (var contabilSaldoInicial in listContaContabilSaldoInicial)
                    {
                        var lancamentoSalvo =
                            saldosList.FirstOrDefault(x => x.ContaContabil.Id == contabilSaldoInicial.Id);
                        if (lancamentoSalvo == null || lancamentoSalvo.Valor != contabilSaldoInicial.Valor)
                        {
                            repositorio.Salvar(contabilSaldoInicial);
                            AjustarSaldoInicial(repositorio, contabilSaldoInicial);
                        }
                    }


                    //Remver nao existentes
                    var saldosNaoExistentes =
                        saldosList.Where(x => !listContaContabilSaldoInicial.Select(y => y.Id).Contains(x.Id));
                    foreach (var contaContabilSaldoInicial in saldosNaoExistentes)
                    {
                        contaContabilSaldoInicial.Valor = 0;
                        repositorio.Salvar(contaContabilSaldoInicial);
                        AjustarSaldoInicial(repositorio, contaContabilSaldoInicial);
                    }

                    repositorio.Session.Transaction.Commit();


                    //lancar diferenca entre ativo e passivo
                    decimal totalAtivo =
                        listContaContabilSaldoInicial.Where(x => x.ContaContabil.Codigo.StartsWith("1."))
                            .Sum(
                                x =>
                                    x.ContaContabil.TipoContaContabil.GetValueOrDefault(EnumTipoContaContabil.Credora) ==
                                    EnumTipoContaContabil.Credora
                                        ? x.Valor * -1
                                        : x.Valor);

                    decimal totalPassivo =
                        listContaContabilSaldoInicial.Where(x => x.ContaContabil.Codigo.StartsWith("2."))
                            .Sum(
                                x =>
                                    x.ContaContabil.TipoContaContabil.GetValueOrDefault(EnumTipoContaContabil.Credora) ==
                                    EnumTipoContaContabil.Devedora
                                        ? x.Valor * -1
                                        : x.Valor);

                    if (totalAtivo != totalPassivo)
                    {
                        decimal diferenca = totalAtivo - totalPassivo;
                        var saldoInicialLucroPrejuizo =
                            listContaContabilSaldoInicial.FirstOrDefault(
                                x => x.ContaContabil.LucroPrejuizoAcumulado == EnumSimNao.Sim);

                        if (saldoInicialLucroPrejuizo != null)
                        {
                            saldoInicialLucroPrejuizo.Valor = diferenca;
                            repositorio.Salvar(saldoInicialLucroPrejuizo);
                            AjustarSaldoInicial(repositorio, saldoInicialLucroPrejuizo);
                        }
                        else
                        {
                            ContaContabilSaldoInicial contaContabilSaldoInicial = new ContaContabilSaldoInicial();
                            contaContabilSaldoInicial.ContaContabil =
                                repositorio.Buscar<ContaContabil>(
                                    "From ContaContabil Where LucroPrejuizoAcumulado = :sim",
                                    new[] { new HqlParameter("sim", EnumSimNao.Sim), }).FirstOrDefault();
                            if (contaContabilSaldoInicial.ContaContabil == null)
                                throw new Exception("Não foi definido a conta de lucro ou prejuízo acumulado");
                            contaContabilSaldoInicial.Valor = diferenca;
                            contaContabilSaldoInicial.Empresa = empresa;
                            repositorio.Salvar(contaContabilSaldoInicial);
                            AjustarSaldoInicial(repositorio, contaContabilSaldoInicial);
                        }
                    }

                    return listContaContabilSaldoInicial;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static void AjustarSaldoInicial(IRepositorio repositorio, ContaContabilSaldoInicial contaContabilSaldoInicial)
        {
            var parameters = new List<HqlParameter>();
            parameters.Add(new HqlParameter("saldoInicial", contaContabilSaldoInicial.Id));

            var lancamentosExistentes = repositorio.Buscar<ContaContabilSaldo>(
                "From ContaContabilSaldo ccs Where ccs.ContaContabilSaldoInicial.Id = :saldoInicial", parameters);

            foreach (var lancamentosExistente in lancamentosExistentes)
            {
                repositorio.Deletar(lancamentosExistente);
            }

            ContaContabilSaldo saldo = new ContaContabilSaldo();
            saldo.ContaContabilSaldoInicial = contaContabilSaldoInicial;
            saldo.ContaContabil = contaContabilSaldoInicial.ContaContabil;
            saldo.Empresa = contaContabilSaldoInicial.Empresa;
            saldo.Data = new DateTime(1800, 1, 1);
            //if (eventoOperacao.Tipo == EnumEventoOperacaoTipo.Debitar) saldo.Saldo = eventoLancamento.Valor * -1;
            saldo.Saldo = contaContabilSaldoInicial.Valor;
            ContaContabil contaContabil = repositorio.BuscarId<ContaContabil>(saldo.ContaContabil.Id);
            if (contaContabil.TipoContaContabil.GetValueOrDefault(EnumTipoContaContabil.Credora) ==
                EnumTipoContaContabil.Devedora)
                saldo.Saldo *= -1;
            repositorio.Salvar(saldo);

        }

        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var contaContabilSaldoInicial = repositorio.BuscarId<ContaContabilSaldoInicial>(id);
                    repositorio.Deletar(contaContabilSaldoInicial);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
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
                                                " WHERE ccsi.Empresa.Id = :empresaId " +
                                                " AND ccsi.ContaContabil = 71 " +
                                                " AND ccsi.Valor = 0 ";

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
                                                " WHERE ccs.Empresa.Id = :empresaId " +
                                                " AND ccs.ContaContabil = 71 " +
                                                " AND ccs.Saldo = 0 ";

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


        public static ContaContabilSaldoInicial CarregaContaHelp(int idConta)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idConta", idConta));

                    string hql = "SELECT cc.Help " +
                                 "FROM ContaContabilSaldoInicial ccs " +
                                 "INNER JOIN ccs.ContaContabil cc " +
                                 "WHERE ccs.ContaContabil = :idConta";

                    var contahelp = repositorio.SeekByHqlObject(hql, parameters).First()[0].ToString();

                    return new ContaContabilSaldoInicial { ContaContabil = new ContaContabil { Help = contahelp } };


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static ContaContabilSaldoInicial CarregaDataSaldoInicial(int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    const string hql = "FROM ContaContabilSaldoInicial ccsi " +
                                 "WHERE ccsi.Empresa = :idEmpresa";

                    var result = repositorio.Buscar<ContaContabilSaldoInicial>(hql, parameters).FirstOrDefault();

                    return result;


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
