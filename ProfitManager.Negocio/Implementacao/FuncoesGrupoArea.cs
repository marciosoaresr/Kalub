using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesGrupoArea
    {

        public static List<GrupoArea> Load()
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    const string hql = "FROM GrupoArea ORDER BY Codigo ";

                    var listArea = repositorio.Buscar<GrupoArea>(hql, null).ToList();

                    return listArea;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static GrupoArea Load(int idGrupoArea)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var area = repositorio.BuscarId<GrupoArea>(idGrupoArea);

                    return area;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
