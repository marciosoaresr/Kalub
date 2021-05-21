using System;
using System.Collections.Generic;
using System.Linq;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class Funcoes
    {
        public static string StringLike(EnumOpcaoPesquisa opcaoPesquisa, string stringBusca)
        {
            stringBusca = stringBusca.ToUpper();
            //Replaces para evitar erros quando digita certos caracteres especiais na quicksearch
            stringBusca = stringBusca.Replace("\\", "");
            stringBusca = stringBusca.Replace("\'", "");

            switch (opcaoPesquisa)
            {
                case EnumOpcaoPesquisa._Like:
                    {
                        return " Like '%" + stringBusca + "'";
                    }
                case EnumOpcaoPesquisa._Like_:
                    {
                        return " Like '%" + stringBusca + "%'";
                    }
                case EnumOpcaoPesquisa.Like_:
                    {
                        return " Like '" + stringBusca + "%'";
                    }
                default:
                    {
                        return " Like '%" + stringBusca + "%'";
                    }
            }
        }

        public static void HqlParametroIn(IEnumerable<int> ids, ref string strHql, ref List<HqlParameter> parameters,
            string propriedade, bool notIn = false)
        {
            string nomeLista = GerarStringAleatoria(10);

            int[] arr = ids.ToArray();
            int qtd = arr.Length;
            string strNotIn = " IN ";
            if (notIn) strNotIn = " NOT IN ";
            if (qtd < 1000)
            {
                strHql += " AND " + propriedade + strNotIn + " (:" + nomeLista + "00) ";
                parameters.Add(new HqlParameter(nomeLista + "00", arr));
                return;
            }
            int cont = (qtd / 1000) + 1;
            string strHqlIn = string.Empty;
            int contador = 0;

            while (contador < cont)
            {
                List<int> param = new List<int>();
                int inicial = contador * 1000;
                int final = inicial + 1000;
                if (final > arr.Length) final = arr.Length;
                for (int i = inicial; i < final; i++)
                {
                    if (i > arr.Length) break;
                    param.Add(arr[i]);
                }

                if (param.Count == 0) break;
                string inPar = nomeLista + contador;
                parameters.Add(new HqlParameter(inPar, param));
                if (!string.IsNullOrEmpty(strHqlIn))
                    strHqlIn += " OR ";
                strHqlIn += " " + propriedade + strNotIn + " (:" + inPar + ") ";

                contador++;
            }

            strHql += " AND ( " + strHqlIn + ") ";
        }

        public static string GerarStringAleatoria(int tamanhoMaximo)
        {
            char[] ch = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            Random r = new Random(Guid.NewGuid().GetHashCode());
            int stringLength = r.Next(5, tamanhoMaximo);
            string nomeLista = string.Empty;

            for (int i = 0; i < stringLength; i++)
            {
                nomeLista += ch[r.Next(0, ch.Length - 1)];
            }
            return nomeLista;
        }
    }
}
