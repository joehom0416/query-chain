using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuaryChain.Extensions
{
    public static class QueryExtension
    {
     
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">boolean value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, bool value)
        {
            return query.AddParameter(parameterName, value, DbType.Boolean);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">DateTime value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, DateTime value)
        {
            return query.AddParameter(parameterName, value, DbType.DateTime);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">Null value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, DBNull value)
        {
            return query.AddParameter(parameterName, "", DbType.String);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">Decimal value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, Decimal value)
        {
            return query.AddParameter(parameterName, value, DbType.Decimal);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">Double value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, Double value)
        {
            return query.AddParameter(parameterName, value, DbType.Double);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">int32 value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, Int32 value)
        {
            return query.AddParameter(parameterName, value, DbType.Int32);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">int64 value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, Int64 value)
        {
            return query.AddParameter(parameterName, value, DbType.Int64);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">int16 value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, Int16 value)
        {
            return query.AddParameter(parameterName, value, DbType.Int16);
        }
        /// <summary>
        /// Add Input Parameter
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="value">string value</param>
        /// <returns>return self</returns>
        public static Query AddParameter(this Query query, string parameterName, string value)
        {
            return query.AddParameter(parameterName, value, DbType.String);
        }
  
    }
}
