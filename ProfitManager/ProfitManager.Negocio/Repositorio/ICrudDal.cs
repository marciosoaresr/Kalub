using System.Collections.Generic;

namespace ProfitManager.Negocio.Repositorio
{
    public interface ICrudDal<T>
    {
        void Inserir(T entidade);
        void Atualizar (T entidade);
        void Deletar (T entidade);
        T BuscarId (T entidade);
        List<T> Buscar();
    }
}