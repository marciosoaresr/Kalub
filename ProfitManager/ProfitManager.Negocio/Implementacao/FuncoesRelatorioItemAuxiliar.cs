using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.EntidadeAuxiliar;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesRelatorioItemAuxiliar
    {
        public static List<RelatorioItemAuxiliar> BuildReport(int idRelatorio, DateTime dataInicial, DateTime dataFinal,
            int idEmpresa)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idRelatorio", idRelatorio));

                    //                              0       1         2          3               4
                    string hqlRelItem = " SELECT ri.Id, ri.Codigo, ri.Nome, ri.Condicional, ri.Negrito, " +
                    //                       5         6         7             8              9
                                        " pri.Id, pri.Codigo, pri.Nome, pri.Condicional, pri.Negrito " +
                                        " FROM RelatorioItem ri " +
                                        " LEFT JOIN ri.Parent pri " +
                                        " WHERE ri.Relatorio = :idRelatorio ";

                    var listRelatorioItemObject = repositorio.SeekByHqlObject(hqlRelItem, parameters);

                    List<RelatorioItem> listRelatorioItem = ConvertListObjectToLisRelatorioItem(listRelatorioItemObject);

                    parameters.Clear();
                    //                                            0       1
                    string hqlContaContabilRelItem = " SELECT ccri.Id, ri.Id, " +
                    //                                   2         3        4
                                                     " cc.Id, cc.Codigo, cri.Nome  " +
                                                     " FROM ContaContabilRelatorioItem ccri " +
                                                     " INNER JOIN ccri.ContaContabil cc " +
                                                     " INNER JOIN ccri.RelatorioItem ri " +
                                                     " INNER JOIN cc.CampoNome cri " +
                                                     " WHERE 1 = 1 ";

                    Funcoes.HqlParametroIn(listRelatorioItem.Select(x => x.Id).ToArray(), ref hqlContaContabilRelItem,
                        ref parameters, "ccri.RelatorioItem");

                    var listContaContabilRelItemObject = repositorio.SeekByHqlObject(hqlContaContabilRelItem, parameters);

                    parameters.Clear();
                    parameters.Add(new HqlParameter("dataInicial", dataInicial));
                    parameters.Add(new HqlParameter("dataFinal", dataFinal));
                    parameters.Add(new HqlParameter("tipo", EnumEventoOperacaoTipo.Debitar));
                    parameters.Add(new HqlParameter("idEmpresa", idEmpresa));

                    string hqlSaldoContaContabil = " SELECT " +
                                                   " cc.Id, Sum(el.Valor * CASE WHEN eo.Tipo = :tipo THEN - 1 ELSE 1 END) " +
                                                   " + COALESCE((SELECT ccsi.Valor FROM ContaContabilSaldoInicial ccsi WHERE ccsi.ContaContabil = cc.Id), 0) " +
                                                   " FROM " +
                                                   " EventoLancamento el " +
                                                   " INNER JOIN el.Evento e, " +
                                                   " EventoOperacao eo " +
                                                   " INNER JOIN eo.ContaContabil cc " +
                                                   " WHERE " +
                                                   " eo.Evento = e.Id AND " +
                                                   " el.Data >= :dataInicial AND el.Data <= : dataFinal " +
                                                   " AND el.Empresa =: idEmpresa ";

                    Funcoes.HqlParametroIn(listContaContabilRelItemObject.Select(x => Convert.ToInt32(x[2])).ToArray(),
                        ref hqlSaldoContaContabil, ref parameters, "cc.Id");

                    hqlSaldoContaContabil = string.Concat(hqlSaldoContaContabil, " GROUP BY cc.Id ");

                    var listSaldoContaContabilObject = repositorio.SeekByHqlObject(hqlSaldoContaContabil, parameters);

                    List<RelatorioItemAuxiliar> listRelatorioItemAux = CalcularSaldoContabil(listRelatorioItem,
                        listContaContabilRelItemObject, listSaldoContaContabilObject);

                    SetNodeDepth(ref listRelatorioItemAux);                    

                    CalcularSaldoItensTotalizadores(ref listRelatorioItemAux);

                    return listRelatorioItemAux;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        
        public static List<RelatorioItem> ConvertListObjectToLisRelatorioItem(List<List<object>> listRelatorioItemObject)
        {
            var listRelatorioItem = new List<RelatorioItem>();

            foreach (var relatorioItemObj in listRelatorioItemObject)
            {
                var relatorioItem = new RelatorioItem
                {
                    Id = Convert.ToInt32(relatorioItemObj[0]),
                    Codigo = relatorioItemObj[1].ToString(),
                    Nome = relatorioItemObj[2].ToString(),
                    Condicional = (EnumSimNao)Convert.ToChar(relatorioItemObj[3]),
                    Negrito = (EnumSimNao)Convert.ToChar(relatorioItemObj[4]),
                };

                if (relatorioItemObj[5] != null)
                {
                    relatorioItem.Parent = new RelatorioItem
                    {
                        Id = Convert.ToInt32(relatorioItemObj[5]),
                        Codigo = relatorioItemObj[6].ToString(),
                        Nome = relatorioItemObj[7].ToString(),
                        Condicional = (EnumSimNao)Convert.ToChar(relatorioItemObj[8]),
                        Negrito = (EnumSimNao)Convert.ToChar(relatorioItemObj[9]),
                    };
                }

                listRelatorioItem.Add(relatorioItem);
            }

            return listRelatorioItem;
        }

        /// <summary>
        /// Calcula o saldo contábil de todos itens de relaórios que possuem contas
        /// </summary>
        /// <param name="listRelatorioItem"></param>
        /// <param name="listContaContabilRelItemObject"></param>
        /// <param name="listSaldoContaContabilObject"></param>
        /// <returns>Lista de RelatorioItemAuxiliar</returns>
        public static List<RelatorioItemAuxiliar> CalcularSaldoContabil(List<RelatorioItem> listRelatorioItem,
            List<List<object>> listContaContabilRelItemObject, List<List<object>> listSaldoContaContabilObject)
        {
            var listRelatorioItemAux = new List<RelatorioItemAuxiliar>();

            foreach (var relItem in listRelatorioItem)
            {
                var relatorioItemAux = new RelatorioItemAuxiliar();

                if (listContaContabilRelItemObject.All(x => Convert.ToInt32(x[1]) != relItem.Id))
                    relatorioItemAux.HasChild = true;


                relatorioItemAux.RelatorioItemId = relItem.Id;
                relatorioItemAux.Nome = relItem.Nome;
                relatorioItemAux.Codigo = relItem.Codigo;
                relatorioItemAux.Condicional = relItem.Condicional;
                relatorioItemAux.Negrito = relItem.Negrito;

                if (relItem.Parent != null)
                {
                    relatorioItemAux.ParentId = relItem.Parent.Id;
                    relatorioItemAux.CodigoParent = relItem.Parent.Codigo;
                    relatorioItemAux.NomeParent = relItem.Parent.Nome;
                }

                foreach (var contaContabilRelatorioItem in
                        listContaContabilRelItemObject.Where(x => Convert.ToInt32(x[1]) == relItem.Id))
                {
                    relatorioItemAux.Valor += listSaldoContaContabilObject.Where(
                       x => Convert.ToInt32(x[0]) == Convert.ToInt32(contaContabilRelatorioItem[2]))
                       .Sum(x => Convert.ToDecimal(x[1]));
                }

                listRelatorioItemAux.Add(relatorioItemAux);
            }


            return listRelatorioItemAux;
        }

        /// <summary>
        /// Atribui a cada nó da estrutura sua profundidade dentro do nível de uma árvore
        /// </summary>
        /// <param name="listRelatorioItemAux"></param>
        public static void SetNodeDepth(ref List<RelatorioItemAuxiliar> listRelatorioItemAux)
        {
            foreach (var relatorioItemAuxiliar in listRelatorioItemAux)
            {
                if (relatorioItemAuxiliar.ParentId == 0)
                {
                    relatorioItemAuxiliar.Profundiade = 0;
                }
                else
                {
                    var profundidade = 0;
                    int parentId = relatorioItemAuxiliar.ParentId;
                    while (parentId != 0)
                    {
                        profundidade++;

                        var relItemParent =
                            listRelatorioItemAux.FirstOrDefault(
                                x => x.RelatorioItemId == parentId);

                        if (relItemParent != null)
                        {
                            parentId = relItemParent.ParentId;
                        }

                    }

                    relatorioItemAuxiliar.Profundiade = profundidade;
                }
            }
        }

        /// <summary>
        /// Calcula o valor de cada item de relatório que não possue contas
        /// </summary>
        /// <param name="listRelatorioItemAux"></param>
        public static void CalcularSaldoItensTotalizadores(ref List<RelatorioItemAuxiliar> listRelatorioItemAux)
        {
            //Importante manter a ordem descendente, pois começa calcular os itens de cima para baixo na estrutra de árvore
            foreach (var relatorioItemAuxiliar in
                            listRelatorioItemAux.Where(rel => rel.HasChild).OrderByDescending(rel => rel.Profundiade))
            {
                relatorioItemAuxiliar.Valor +=
                        listRelatorioItemAux.Where(x => x.ParentId == relatorioItemAuxiliar.RelatorioItemId)
                            .Sum(x => x.Valor);
            }
        }


    }
}
