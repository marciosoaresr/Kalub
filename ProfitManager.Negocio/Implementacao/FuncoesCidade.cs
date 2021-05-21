using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesCidade
    {
        public static List<Cidade> LoadByName(string nomeCidade)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    string hql = "FROM Cidade WHERE UPPER(TRIM(Nome)) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like_, nomeCidade) + " OR UPPER(TRIM(Nome)) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeCidade);

                    var listCidade = repositorio.Buscar<Cidade>(hql, null).ToList();

                    return listCidade;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Cidade Load(int idCidade)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idCidade", idCidade));

                    string hql = "FROM Cidade WHERE Id = :idCidade ";

                    var cidade = repositorio.Buscar<Cidade>(hql, parameters).FirstOrDefault();

                    return cidade;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Cidade> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Cidade ";

                    var listCidade = repositorio.Buscar<Cidade>(hql, null).ToList();

                    return listCidade;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
