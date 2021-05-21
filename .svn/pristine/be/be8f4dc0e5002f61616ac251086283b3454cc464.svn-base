using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class EventoOperacao: EntidadeBase
    {
        public virtual Evento Evento { get; set; }
        public virtual ContaContabil ContaContabil { get; set; }
        public virtual EnumEventoOperacaoTipo Tipo { get; set; }
 
        public override bool Equals(object obj)
        {
            var eventoOperacao = obj as EventoOperacao;

            if (eventoOperacao == null || GetType() != eventoOperacao.GetType())
            {
                return false;
            }

            if (eventoOperacao.Id != 0)
            {
                return this.Id == eventoOperacao.Id;
            }
            else
            {
                if (this.Evento != null && eventoOperacao.Evento != null 
                    && this.ContaContabil != null && eventoOperacao.ContaContabil != null)
                {
                    return this.Evento.Equals(eventoOperacao.Evento) && this.ContaContabil.Equals(eventoOperacao.ContaContabil)
                        && this.Tipo == eventoOperacao.Tipo;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashEvento = (this.Evento == null) ? 0 : this.Evento.Id.GetHashCode();
            int hashContaContabil = (this.ContaContabil == null) ? 0 : this.ContaContabil.Id.GetHashCode();
            int hashTipo = this.Tipo.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashEvento + hashContaContabil + hashTipo);
        }
    }
}
