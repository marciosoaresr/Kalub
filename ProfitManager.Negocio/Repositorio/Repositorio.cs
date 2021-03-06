using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Impl;
using ProfitManager.Fabrica.Entidade;

namespace ProfitManager.Negocio.Repositorio
{
    public class Repositorio : IDisposable, IRepositorio
    {
        public ISession Session { get; set; }
        public void Salvar(EntidadeBase entidade)
        {
            if (entidade.Id == 0)
                entidade.DataHoraCriacao = DateTime.Now;
            else
                entidade.DataHoraAlteracao = DateTime.Now;

            if (entidade.DataHoraCriacao == Convert.ToDateTime("01/01/0001"))
                entidade.DataHoraCriacao = DateTime.Now;

            if (entidade.DataHoraCriacao == null)
                entidade.DataHoraCriacao = DateTime.Now;

            Session.SaveOrUpdate(entidade);
        }

        public void Deletar(EntidadeBase entidade)
        {
            Session.Delete(entidade);
            Session.Flush();
        }

        public T BuscarId<T>(int id)
        {
            return Session.Get<T>(id);
        }

        public IList<T> Buscar<T>(string hql, IList<HqlParameter> parameters )
        {
            var query = Session.CreateQuery(hql);
            SetQueryParameters(query, parameters);

            return query.List<T>();
        }

        private void SetQueryParameters(IQuery query, IList<HqlParameter> parameters)
        {
            if (parameters == null) return;
            foreach (HqlParameter parameter in parameters)
            {
                HqlParameter.HqlParameterType ParameterType = parameter.ParameterType;
                string ParameterName = parameter.ParameterName;
                object Value = parameter.Value;

                if (ParameterType == HqlParameter.HqlParameterType.AnsiString)
                    query.SetAnsiString(ParameterName, (string)Value);
                else if (ParameterType == HqlParameter.HqlParameterType.Binary)
                    query.SetBinary(ParameterName, (byte[])Value);
                else if (ParameterType == HqlParameter.HqlParameterType.Boolean)
                    query.SetBoolean(ParameterName, (bool)Value);
                else if (ParameterType == HqlParameter.HqlParameterType.Byte)
                    query.SetByte(ParameterName, (byte)Value);
                else if (ParameterType == HqlParameter.HqlParameterType.Caracter)
                    query.SetCharacter(ParameterName, (char)Value);
                else if (ParameterType == HqlParameter.HqlParameterType.DateTime)
                    query.SetDateTime(ParameterName, (DateTime)Value);
                else if (ParameterType == HqlParameter.HqlParameterType.Decimal)
                    query.SetDecimal(ParameterName, (decimal)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Double)
                    query.SetDouble(ParameterName, (double)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Guid)
                    query.SetGuid(ParameterName, (Guid)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Int16)
                    query.SetInt16(ParameterName, (Int16)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Int32)
                    query.SetInt32(ParameterName, (Int32)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Int64)
                    query.SetInt64(ParameterName, (Int64)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType
                                               .String)
                    query.SetString(ParameterName, (string)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.Time)
                    query.SetTime(ParameterName, (DateTime)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.TimeStamp)
                    query.SetTimestamp(ParameterName,
                                       (DateTime)Value);
                else if (ParameterType ==
                         HqlParameter.HqlParameterType.List)
                {
                    int quantidade =
                        ((IEnumerable)Value).Cast<object>()
                                             .Count();
                    if (Value == null || quantidade == 0)
                    {
                        Value = new List<object> { 0 };
                    }

                    query.SetParameterList(ParameterName,
                                           (IEnumerable)
                                           Value);
                }
            }
        }

        public List<List<object>> SeekByHqlObject(string hql, IList<HqlParameter> parameters, int maxResult = 0)
        {
            //Versao 1
            //((ISession)Session).Connection.CreateCommand();

            IQuery query = null;

            query = ((ISession)Session).CreateQuery(hql);

            //query.SetCacheable(true);
            if (parameters != null) SetQueryParameters(query, parameters);

            if (maxResult > 0)
                query.SetMaxResults(maxResult);

            List<object> dataList = query.List<object>(true).ToList();

            List<List<object>> objReturn = new List<List<object>>();

            foreach (var item in dataList)
            {
                if (item is object[])
                {
                    objReturn.Add(((object[]) item).ToList());
                }
                else if (item is DataTable)
                {
                    objReturn.AddRange(from DataRow dr in ((DataTable) item).Rows
                        select dr.ItemArray.Select(itemArr => itemArr == DBNull.Value ? null : itemArr).ToList());
                }
                else
                {
                    object objValue = item;
                    if (objValue == DBNull.Value) objValue = null;
                    List<object> obj = new List<object>();
                    obj.Add(objValue);
                    objReturn.Add(obj);
                }
            }
            return objReturn;
        }

        public void Dispose()
        {
            
        }
    }
}
