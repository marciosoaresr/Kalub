using System;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEventoCategoriaEmpresa
    {
        public static EventoCategoriaEmpresa Load(int id)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var eventoCategoriaEmpresa = repositorio.BuscarId<EventoCategoriaEmpresa>(id);

                    return eventoCategoriaEmpresa;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

    }
}
