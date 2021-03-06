using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Evento: EntidadeBase
    {
        [DisplayName("Área")]
        [Required (ErrorMessage = "Informe a área")]
        public virtual Area Area { get; set; }

        [DisplayName("SubÁrea")]
        [Required(ErrorMessage = "Informe a subarea")]
        public virtual SubArea SubArea { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "Informe o código")]
        public virtual string Codigo { get; set; }

        [DisplayName("Nome")]
        [Required (ErrorMessage = "Informe o nome")]
        public virtual Campo CampoNome { get; set; }

        [DisplayName("Help")]
        [Required (ErrorMessage = "Informe o help")]
        public virtual Campo CampoHelp { get; set; }

        [DisplayName("Tipo de Lançamento")]
        [Required(ErrorMessage = "Informe o tipo de lançamento")]
        public virtual EnumTipoEventoLancamento TipoEventoLancamento { get; set; }

        [DisplayName("Restringe Categoria Empresa?")]
        [Required(ErrorMessage = "Informe se restringe categoria empresa")]
        public virtual bool RestringeCategoriaEmpresa { get; set; }

        [DisplayName("Mais Usado?")]
        [Required(ErrorMessage = "Informe se é mais usado")]
        public virtual EnumMaisUsado MaisUsado { get; set; }

        [DisplayName("Apuração de resultado?")]
        [Required(ErrorMessage = "Informe se é o evento é de apuração de resultado")]
        public virtual EnumSimNao ApuracaoResultado { get; set; }

        public virtual EnumTipoDFC TipoDFC { get; set; }

        [DisplayName("Help")]
        [Required(ErrorMessage = "Informe o texto do help")]
        [AllowHtml]
        public virtual string Help { get; set; }
        public virtual int Ordem { get; set; }

        IList<EventoOperacao> _ListEventoOperacao = new List<EventoOperacao>();

        IList<EventoCategoriaEmpresa> _ListEventoCategoriaEmpresa = new List<EventoCategoriaEmpresa>();

        public virtual EventoOperacao[] ListEventoOperacao
        {
            get { return _ListEventoOperacao.ToArray(); }
        }

        public virtual EventoCategoriaEmpresa[] ListEventoCategoriaEmpresa
        {
            get { return _ListEventoCategoriaEmpresa.ToArray(); }
        }

        public virtual string NomeNormalizado
        {
            get
            {
                return Codigo + " - " + CampoNome.Nome;
            }
        }

        public virtual void AddListEventoOperacao(EventoOperacao item)
        {
            if(_ListEventoOperacao.Any(x => x.ContaContabil.Id == item.ContaContabil.Id))
                return;

            item.Evento = this;
            _ListEventoOperacao.Add(item);
        }

        public virtual void AddListEventoCategoria(EventoCategoriaEmpresa item)
        {
            if (_ListEventoCategoriaEmpresa.Any(x => x.CategoriaEmpresa.Id == item.CategoriaEmpresa.Id))
                return;

            item.Evento = this;
            _ListEventoCategoriaEmpresa.Add(item);
        }

        public virtual void RemoveItemListEventoOperacao(int hashCodeObject)
        {
            if (_ListEventoOperacao.All(x => x.GetHashCode() != hashCodeObject))
                return;

            _ListEventoOperacao.Remove(_ListEventoOperacao.First(x => x.GetHashCode() == hashCodeObject));
        }

        public virtual void RemoveItemListEventoCategoria(int hashCodeObject)
        {
            if (_ListEventoCategoriaEmpresa.All(x => x.GetHashCode() != hashCodeObject))
                return;

            _ListEventoCategoriaEmpresa.Remove(_ListEventoCategoriaEmpresa.First(x => x.GetHashCode() == hashCodeObject));
        }
        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashArea = (this.Area == null) ? 0 : this.Area.GetHashCode();
            int hashCampoNome = (this.CampoNome == null) ? 0 : this.CampoNome.GetHashCode();
            int hashCampoHelp = (this.CampoHelp == null) ? 0 : this.CampoHelp.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashArea + hashCampoHelp + hashCampoNome);
        }

    }
}
