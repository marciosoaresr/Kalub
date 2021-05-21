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
    public static class FuncoesEstado
    {
        public static List<Estado> LoadByName(string nomeEstado)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    string hql = "FROM Estado WHERE UPPER(TRIM(Nome)) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like_, nomeEstado) + " OR UPPER(TRIM(Nome)) " +
                                 Funcoes.StringLike(EnumOpcaoPesquisa._Like, nomeEstado);

                    var listEstado = repositorio.Buscar<Estado>(hql, null).ToList();

                    return listEstado;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Estado Load(int idEstado)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idEstado", idEstado));

                    string hql = "FROM Estado WHERE Id = :idEstado ";

                    var estado = repositorio.Buscar<Estado>(hql, parameters).FirstOrDefault();

                    return estado;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Estado> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM Estado ";

                    var listEstado = repositorio.Buscar<Estado>(hql, null).ToList();

                    return listEstado;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
