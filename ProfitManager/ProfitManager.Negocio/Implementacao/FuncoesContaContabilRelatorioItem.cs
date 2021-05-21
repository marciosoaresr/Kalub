using System;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesContaContabilRelatorioItem
    {

        public static ContaContabilRelatorioItem Load(int idContaContabilRelatorioItem)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var relatorio = repositorio.BuscarId<ContaContabilRelatorioItem>(idContaContabilRelatorioItem);

                    return relatorio;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
            
        }


    }
}
