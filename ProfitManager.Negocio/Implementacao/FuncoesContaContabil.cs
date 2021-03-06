using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesContaContabil
    {
        public static void Excluir(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    var contaContabil = repositorio.BuscarId<ContaContabil>(id);
                    repositorio.Deletar(contaContabil);

                    repositorio.Session.Transaction.Commit();
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<ContaContabil> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM ContaContabil ORDER BY Codigo ";

                    var listContaContabil = repositorio.Buscar<ContaContabil>(hql, null).ToList();

                    return listContaContabil;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static ContaContabil Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parametrers = new List<HqlParameter>();
                    parametrers.Add(new HqlParameter("idContaContabil", id));

                    const string hql = "FROM ContaContabil WHERE Id = :idContaContabil ";

                    var contaContabil = repositorio.Buscar<ContaContabil>(hql, parametrers).FirstOrDefault();

                    return contaContabil;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
        
        public static ContaContabil Salvar(ContaContabil contaContabil)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    if (contaContabil.LucroPrejuizoAcumulado == EnumSimNao.Sim &&
                        contaContabil.ExigeSaldoinicial == EnumExigeSaldoinicial.Sim)
                        throw new Exception("A conta de lucro ou prejuizo não pode exigir saldo inicial");
                    repositorio.Salvar(contaContabil.CampoNome);
                    repositorio.Salvar(contaContabil.CampoHelp);
                    repositorio.Salvar(contaContabil);

                    //validar se tem mais de uma conta como lucro ou prejuizo acumulado
                    var listContaContabil = LoadWithSeekByHqlObject().Where(x => x.LucroPrejuizoAcumulado == EnumSimNao.Sim);
                    if (listContaContabil.Count() > 1)
                        throw new Exception("Existe mais de um conta como lucro ou prejuizo acumulado");

                    repositorio.Session.Transaction.Commit();

                    return contaContabil;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }

        public static List<ContaContabil> Load(string codigo)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                { 
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("codigoConta", codigo.Trim()));

                    string hql = "FROM ContaContabil WHERE Codigo " + Funcoes.StringLike(EnumOpcaoPesquisa._Like, codigo) +
                        " OR TRIM(Codigo) = :codigoConta ";

                    var listContaContabil = repositorio.Buscar<ContaContabil>(hql, parameters).ToList();


                    return listContaContabil;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


        public static List<ContaContabil> LoadWithSeekByHqlObject()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {

                    //                            0       1        2
                    const string hql = " SELECT c.Id, c.Codigo, cn.Nome, c.ExigeSaldoinicial, c.LucroPrejuizoAcumulado, c.NotaExplicativaContaContabil, " +
                                       " ccsg.Codigo as SubGrupoCodigo, cnsg.Nome as SubGrupoNome, " +
                                       " ccg.Codigo as GrupoCodigo, cng.Nome as GrupoNome, ccsg.NotaExplicativaSubGrupo, ccg.NotaExplicativaGrupo, ecampohelp.Nome, c.Help " +
                                       " FROM ContaContabil c " +
                                       " INNER JOIN c.CampoNome cn " +
                                       " INNER JOIN c.CampoHelp ecampohelp " +
                                       " INNER JOIN c.ContaContabilSubGrupo ccsg " +
                                       " INNER JOIN ccsg.CampoNome cnsg " +
                                       " INNER JOIN ccsg.ContaContabilGrupo ccg " +
                                       " INNER JOIN ccg.CampoNome cng ";
                    

                    var listContaContabilObject = repositorio.SeekByHqlObject(hql, null);

                    var listContaContabil =
                        listContaContabilObject.Select(contaContabil => new ContaContabil
                        {
                            Id = Convert.ToInt32(contaContabil[0]),
                            Codigo = Convert.ToString(contaContabil[1]),
                            CampoNome = new Campo
                            {
                                Nome = Convert.ToString(contaContabil[2])
                            },
                            ExigeSaldoinicial = (EnumExigeSaldoinicial)Convert.ToChar(contaContabil[3]),
                            LucroPrejuizoAcumulado = (EnumSimNao)Convert.ToChar(contaContabil[4]),
                            NotaExplicativaContaContabil = Convert.ToString(contaContabil[5]),
                            SubGrupoCodigo = Convert.ToString(contaContabil[6]),
                            SubGrupoNome = Convert.ToString(contaContabil[7]),
                            GrupoCodigo = Convert.ToString(contaContabil[8]),
                            GrupoNome = Convert.ToString(contaContabil[9]),
                            NotaExplicativaGrupo = Convert.ToString(contaContabil[10]),
                            NotaExplicativaSubGrupo = Convert.ToString(contaContabil[11]),
                            Help = Convert.ToString(contaContabil[13]),

                            CampoHelp = new Campo
                            {
                                Nome = Convert.ToString(contaContabil[12])
                            },

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
