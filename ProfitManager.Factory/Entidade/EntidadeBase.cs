using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class EntidadeBase
    {
        [Key]
        [Editable(false)]
        [Range(0, int.MaxValue)]
        public virtual int Id { get; set; }
        public virtual DateTime? DataHoraCriacao { get; set; }
        public virtual DateTime? DataHoraAlteracao { get; set; }

        public virtual void SaveValidate()
        {
            
        }

    }
}
