using System;
using System.ComponentModel.DataAnnotations;


namespace ProfitManager.Fabrica.Entidade
{
    public class EmpresaUsuario: EntidadeBase
    {
        public virtual Empresa Empresa { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "Informe o login")]
        public virtual string Login { get; set; }

        [Required(ErrorMessage = "Informe o email")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public virtual string Senha { get; set; }

        [Required(ErrorMessage = "Informe o cnpj")]
        public virtual string Cnpj { get; set; }


    }
}
