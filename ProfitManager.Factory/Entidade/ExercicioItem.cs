using System;
using System.ComponentModel.DataAnnotations;

namespace ProfitManager.Fabrica.Entidade
{
    public class ExercicioItem : EntidadeBase
    {

        public virtual Exercicio Exercicio { get; set; }

        public virtual string Descricao { get; set; }

        public virtual Evento Evento { get; set; }

        public virtual CategoriaEmpresa CategoriaEmpresa { get; set; }

        public virtual DateTime Data { get; set; }

        public virtual decimal Valor { get; set; }

        public override bool Equals(object obj)
        {
            var exercicioOperacao = obj as ExercicioItem;

            if (exercicioOperacao == null || GetType() != exercicioOperacao.GetType())
            {
                return false;
            }

            if (exercicioOperacao.Id != 0)
            {
                return this.Id == exercicioOperacao.Id;
            }
            else
            {
                if (this.Evento != null && exercicioOperacao.Evento != null
                    && this.Evento != null && exercicioOperacao.Evento != null)
                {
                    return this.Evento.Equals(exercicioOperacao.Evento) && this.Evento.Equals(exercicioOperacao.Evento)
                        ;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashExercicio = (this.Exercicio == null) ? 0 : this.Exercicio.Id.GetHashCode();
            int hashEvento = (this.Evento == null) ? 0 : this.Evento.Id.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashEvento + hashExercicio);
        }
    }
}
