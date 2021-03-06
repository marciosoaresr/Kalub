using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Usuario: EntidadeBase
    {
        public virtual string Login { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Senha { get; set; }
        public virtual string Email { get; set; }
        public virtual Empresa Empresa { get; set; }
        #region Propriedades não mapeadas
        
        public virtual EnumSimNao Administrador { get; set; }

        #endregion


    }
}
