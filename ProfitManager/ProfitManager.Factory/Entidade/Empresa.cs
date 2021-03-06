using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Empresa: EntidadeBase
    {
        //public Empresa()
        //{
        //    EmpresaId = Guid.NewGuid();
        //}
        //public virtual Guid EmpresaId { get ; set; }

        [Required(ErrorMessage = "Informe a nome da emprea")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "Informe o nome fantasia")]
        public virtual string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Informe o cnpj")]
        public virtual string Cnpj { get; set; }

        [Required(ErrorMessage = "Informe o logradouro")]
        public virtual string Logradouro { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "Informe o número")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "Informe o complemento")]
        public virtual string Complemento { get; set; }

        public virtual DateTime DataAbertura { get; set; }

        [Required(ErrorMessage = "Informe o bairro")]
        public virtual string Bairro { get; set; }

        [Required(ErrorMessage = "Informe o cep")]
        public virtual string Cep { get; set; }

        [Required(ErrorMessage = "Informe o email")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Informe o telefone 1")]
        public virtual string Telefone1 { get; set; }

        [Required(ErrorMessage = "Informe o telefone 2")]
        public virtual string Telefone2 { get; set; }

        [Required(ErrorMessage = "Informe a cidade")]
        public virtual Cidade Cidade { get; set; }

        public virtual CategoriaEmpresa CategoriaEmpresa { get; set; }

        public virtual CategoriaEmpresaSecundaria CategoriaEmpresaSecundaria { get; set; }

        public virtual string CategoriaJuridicaEmpresa { get; set; }


        [Required(ErrorMessage = "Informe o percentual do simples nacional")]
        [Display (Name = "Percentual do simples nacional")]
        public virtual decimal SimplesNacional { get; set; }

        [Required(ErrorMessage = "Informe o percentual de custo da venda")]
        [Display (Name = "Percentual de custo da venda")]
        public virtual decimal CustoVenda { get; set; }

        public virtual string PeriodoGratis { get; set; }
        public virtual string PeriodoUsoVencido { get; set; }
        public virtual int CupomPromocional { get; set; }
        public virtual EnumEmpresaStatus Status { get; set; }

        public virtual DateTime? DataAtivacao { get; set; }
        public virtual DateTime? DataUltimaAtivacao { get; set; }
        public virtual int DiaVencimento { get; set; }
        public virtual string Logomarca { get; set; }
        public virtual int TipoPlano { get; set; }
        public virtual string IdAssinatura { get; set; }
        public virtual EnumTipoCadastro TipoCadastro { get; set; }
        public virtual EnumTipoProfessorAluno TipoProfessorAluno { get; set; }
    }
}
