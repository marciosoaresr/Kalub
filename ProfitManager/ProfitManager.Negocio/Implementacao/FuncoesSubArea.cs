using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesSubArea
    {
        public static List<SubArea> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM SubArea ORDER BY Codigo ";

                    var listSubArea = repositorio.Buscar<SubArea>(hql, null).ToList();

                    return listSubArea;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Area Load(int idArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idGrupoArea", idArea));
                    const string hql = "FROM Area WHERE Id = :idGrupoArea ";

                    var area = repositorio.Buscar<Area>(hql, parameters).SingleOrDefault();

                    return area;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static List<Area> LoadByGrupo(int idGrupoArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idGrupoArea", idGrupoArea));

                    const string hql = "FROM Area WHERE GrupoArea = :idGrupoArea ORDER BY Codigo ";

                    var areas = repositorio.Buscar<Area>(hql, parameters).ToList();

                    return areas;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }



    }
}
