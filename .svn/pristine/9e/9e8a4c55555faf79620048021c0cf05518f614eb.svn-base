using System;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Excecao
{
    public class NotificationException : Exception
    {
        public EnumTypeException TypeException { get; set; }

        public NotificationException(string message, EnumTypeException typeException)
            : base(message)
        {
            TypeException = typeException;
        }
    }
}
