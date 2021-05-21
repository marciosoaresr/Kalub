﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class BalancoPatrimonial : EntidadeBase
    {
        public virtual string Codigo { get; set; }
        public virtual Campo CampoNome { get; set; }
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataAtual { get; set; }
        public virtual decimal SaldoAnterior { get; set; }
        public virtual decimal SaldoAtual { get; set; }
        public virtual decimal SaldoAcumulado { get; set; }
        public virtual decimal SaldoMes { get; set; }
        public virtual string SubGrupoCodigo { get; set; }
        public virtual string SubGrupoNome { get; set; }
        public virtual string GrupoCodigo { get; set; }
        public virtual string GrupoNome { get; set; }
        public virtual EnumExigeSaldoinicial ExigeSaldoinicial { get; set; }
    }
}
