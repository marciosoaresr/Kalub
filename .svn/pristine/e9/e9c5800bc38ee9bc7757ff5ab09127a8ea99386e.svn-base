using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public class FuncoesEmpresaRecebimentoTransacao
    {
        public static EmpresaRecebimentoTransacao Salvar(EmpresaRecebimentoTransacao empresaRecebimentoTransacao)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                repositorio.Session.BeginTransaction();
                try
                {
                    repositorio.Salvar(empresaRecebimentoTransacao);
                    repositorio.Session.Transaction.Commit();

                    return empresaRecebimentoTransacao;
                }
                catch (Exception err)
                {
                    repositorio.Session.Transaction.Rollback();
                    throw;
                }
            }
        }



    }
}
