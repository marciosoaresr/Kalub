using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Negocio.Repositorio
{

    [Serializable]
    public class HqlParameter
    {
        [Serializable]
        public enum HqlParameterType
        {
            AnsiString,
            Binary,
            Boolean,
            Byte,
            Caracter,
            DateTime,
            Decimal,
            Double,
            Guid,
            Int16,
            Int32,
            Int64,
            String,
            Time,
            TimeStamp,
            List
        };

        public HqlParameter(string parameterName, object objeto)
        {
            HqlParameterType tipo = HqlParameterType.AnsiString;
            object obj = objeto;
            if (objeto is string)
            {
                tipo = HqlParameterType.AnsiString;
                obj = Convert.ToString(objeto);
            }
            else if (objeto is byte[])
            {
                tipo = HqlParameterType.Binary;
            }
            else if (objeto is IEnumerable)
            {
                tipo = HqlParameterType.List;
            }
            else if (objeto is Boolean)
            {
                tipo = HqlParameterType.Boolean;
                obj = Convert.ToBoolean(objeto);
            }
            else if (objeto is Byte)
            {
                tipo = HqlParameterType.Byte;
                obj = Convert.ToByte(objeto);
            }
            else if (objeto is Char)
            {
                tipo = HqlParameterType.Caracter;
                obj = Convert.ToChar(objeto);
            }
            else if (objeto is DateTime)
            {
                tipo = HqlParameterType.DateTime;
                obj = Convert.ToDateTime(objeto);
            }
            else if (objeto is Decimal)
            {
                tipo = HqlParameterType.Decimal;
                obj = Convert.ToDecimal(objeto);
            }
            else if (objeto is Double)
            {
                tipo = HqlParameterType.Double;
                obj = Convert.ToDouble(objeto);
            }
            else if (objeto is Guid)
            {
                tipo = HqlParameterType.Guid;
            }
            else if (objeto is Int16)
            {
                tipo = HqlParameterType.Int16;
                obj = Convert.ToInt16(objeto);
            }
            else if (objeto is Int32)
            {
                tipo = HqlParameterType.Int32;
                obj = Convert.ToInt32(objeto);
            }
            else if (objeto is Int64)
            {
                tipo = HqlParameterType.Int64;
                obj = Convert.ToInt64(objeto);
            }
            if (objeto is Enum)
            {
                tipo = HqlParameterType.Caracter;
                obj = Convert.ToChar(objeto);
            }

            Construir(tipo, parameterName, obj);
        }

        private void Construir(HqlParameterType type, string parameterName, object value)
        {
            ParameterName = parameterName;
            ParameterType = type;
            Value = value;
        }

        public HqlParameterType ParameterType { get; set; }
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
}
