using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Exercicio : EntidadeBase
    {

        public virtual Empresa Empresa { get; set; }

        public virtual string Usuario { get; set; }

        public virtual string Titulo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual DateTime DataInicio { get; set; }

        public virtual DateTime DataFim { get; set; }

        public virtual CategoriaEmpresa CategoriaEmpresa { get; set; }

        IList<ExercicioItem> _ListExercicioItem = new List<ExercicioItem>();

        public virtual ExercicioItem[] ListExercicioItem
        {
            get { return _ListExercicioItem.ToArray(); }
        }

        public virtual void AddListExercicioItem(ExercicioItem item)
        {
            if (_ListExercicioItem.Any(x => x.CategoriaEmpresa.Id == item.CategoriaEmpresa.Id))
                return;

            item.Exercicio = this;

            _ListExercicioItem.Add(item);
        }

        public virtual void RemoveItemListExercicioOperacao(int hashCodeObject)
        {
            if (_ListExercicioItem.All(x => x.GetHashCode() != hashCodeObject))
                return;

            _ListExercicioItem.Remove(_ListExercicioItem.First(x => x.GetHashCode() == hashCodeObject));
        }

    }
}
