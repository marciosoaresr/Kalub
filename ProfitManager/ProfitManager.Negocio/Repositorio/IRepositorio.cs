

using System.Collections.Generic;
using NHibernate;
using ProfitManager.Fabrica.Entidade;

namespace ProfitManager.Negocio.Repositorio
{
    public interface IRepositorio
    {
        ISession Session { get; set; }
        void Salvar(EntidadeBase entidade);
        void Deletar(EntidadeBase entidade);
        T BuscarId<T>(int id);
        IList<T> Buscar<T>(string hql, IList<HqlParameter> parameters);
        List<List<object>> SeekByHqlObject(string hql, IList<HqlParameter> parameters, int maxResult = 0);

    }
}
