using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuaryChain
{
    /// <summary>
    /// Handle query, parameters
    /// </summary>
    public class Query
    {
        private readonly QueryConnection _dbConnection;
        private readonly string _query;
        private readonly List<SqlParameter> _paramaters;
        private readonly Dictionary<string, string> _parametersReplace;
        public Query(QueryConnection dbConnection, string query)
        {
            _dbConnection = dbConnection;
            _query = query;
            _paramaters=new List<SqlParameter>();
            _parametersReplace=new Dictionary<string, string>();
        }




        #region Parameteres
        /// <summary>
        /// Adds a System.Data.Common.DbParameter item with the specified value to the System.Data.Common.DbParameterCollection.
        ///</summary>
        ///<param name="parameterName">parameter name</param>
        ///<param name="value">value</param>
        ///<param name="dbType">database type</param>
        ///<remarks></remarks>
        public Query AddParameter(string parameterName, object value, DbType dbType)
        {

#if DEBUG
            ValidateAddParameter(dbType, value);
#endif
            SqlParameter dbParameter = new SqlParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.DbType = dbType;
            if ((dbType == DbType.DateTime || dbType == DbType.Date) && !string.IsNullOrEmpty(value + ""))
            {
                DateTime dateTimeValue = DateTime.Parse((string)value);
                dbParameter.Value = dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            else
                dbParameter.Value = value;
            _paramaters.Add(dbParameter);
            return this;
        }
        public Query AddParameter(string parameterName, bool value)
        {
            return AddParameter(parameterName, value, DbType.Boolean);
        }

        public Query AddParameter(string parameterName, DateTime value)
        {
            return AddParameter(parameterName, value, DbType.DateTime);
        }

        public Query AddParameter(string parameterName, DBNull value)
        {
            return AddParameter(parameterName, "", DbType.String);
        }

        public Query AddParameter(string parameterName, Decimal value)
        {
            return AddParameter(parameterName, value, DbType.Decimal);
        }

        public Query AddParameter(string parameterName, Double value)
        {
            return AddParameter(parameterName, value, DbType.Double);
        }

        public Query AddParameter(string parameterName, Int32 value)
        {
            return AddParameter(parameterName, value, DbType.Int32);
        }

        public Query AddParameter(string parameterName, Int64 value)
        {
            return AddParameter(parameterName, value, DbType.Int64);
        }

        public Query AddParameter(string parameterName, Int16 value)
        {
            return AddParameter(parameterName, value, DbType.Int16);
        }

        public Query AddParameter(string parameterName, string value)
        {
            return AddParameter(parameterName, value, DbType.String);
        }

        /// <summary>
        /// Add Parameters require in SQL Condition, for example SeqNr IN (15,16,17), pass in 15,16,17 in list
        /// </summary>
        /// <param name="parameterPrefix">Parameter prefix, for example @Type</param>
        /// <param name="values">List of values in Array</param>
        ///  <param name="dbType">Column Type</param>
        public Query AddParameters(string parameterPrefix, string[] values, DbType dbType)
        {
            List<string> listCollect = new List<string>();
            for (int loopIndex = 0; loopIndex <= values.Count() - 1; loopIndex++)
            {
                AddParameter(string.Concat(parameterPrefix, loopIndex), values[loopIndex], dbType);
                listCollect.Add(string.Concat(parameterPrefix, loopIndex));
            }

            // add the mapping for key and replace data
            if (!_parametersReplace.ContainsKey(parameterPrefix))
                _parametersReplace.Add(parameterPrefix, "");

            _parametersReplace[parameterPrefix] = string.Join(",", listCollect) + "";
            return this;
        }
        #endregion

        #region query


        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <returns>Returns DataTable</returns>
        public DataTable GetDataTable()
        {
            DataTable result = new DataTable();
            _dbConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(GetSqlCommand());
            adapter.Fill(result);
            _dbConnection.Close();
            adapter.Dispose();
            return result;
        }

        /// <summary>
        /// Get DataSet
        /// </summary>
        /// <returns>Returns DataSet</returns>
        public DataSet GetDataSet()
        {
            DataSet result = new DataSet();
            _dbConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(GetSqlCommand());
            adapter.Fill(result);
            _dbConnection.Close();
            adapter.Dispose();
            return result;
        }
        /// <summary>
        /// Get Custom Collection
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <returns>Returns list of models</returns>
        public IList<T> GetCustomCollection<T>() where T : class, new()
        {
            IList<T> result = new List<T>();
            _dbConnection.Open();
            SqlDataReader reader = GetSqlCommand().ExecuteReader();
            while (reader.Read())
            {
                T item = ConvertToObject<T>(reader);
                result.Add(item);
            }
            reader.Close();
            _dbConnection.Close();
            return result;

        }
        /// <summary>
        ///  Executes the query, and returns the first column of the first row in the result
        ///    set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference (Nothing in Visual Basic) if the result set is empty. Returns a maximum of 2033 characters.</returns>
        public Object ExecuteScalar()
        {
            object result;
            _dbConnection.Open();
            SqlCommand cmd = GetSqlCommand();
            result = cmd.ExecuteScalar();
            _dbConnection.Close();
            return result;
        }


#pragma warning disable S1172 // Unused method parameters should be removed
        private T ConvertToObject<T>(SqlDataReader rd) where T : class, new()
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }

        #endregion

        #region non query
        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <returns>return number of row affected</returns>
        public int ExecuteQuery()
        {
          
            SqlCommand cmd = GetSqlCommand();
            _dbConnection.Open(cmd);
            int recordsAffected = cmd.ExecuteNonQuery();
            _dbConnection.Close();
            return recordsAffected;
        }
        /// <summary>
        /// Execute Non query with asynchronous method
        /// </summary>
        /// <returns>returns number of row affected</returns>
        public async Task<int> ExecuteQueryAsync(CancellationToken token)
        {
            SqlCommand cmd= GetSqlCommand();
             await _dbConnection.OpenAsync(cmd, token);

            int recordsAffected = await cmd.ExecuteNonQueryAsync(token);
            _dbConnection.Close();
            return recordsAffected;
        }
        public async Task<int> ExecuteQueryAsync()
        {
            return await ExecuteQueryAsync(CancellationToken.None);
        }
        #endregion

        /// <summary>
        /// Replace SQL parameters from @RefType to @RefType0,@RefType1
        /// </summary>
        /// <returns>returns replace query in String</returns>
        private string GetFinalQuery()
        {
            string queryReplace = _query +"";
            foreach(KeyValuePair<string,string> item in _parametersReplace)
            {
                queryReplace = queryReplace.Replace(item.Key + "", item.Value + "");
            }
            return  queryReplace;
        }

        /// <summary>
        ///  Throw error when use invalid DbType
        ///  </summary>
        /// <param name="type"></param>
        private void ValidateAddParameter(DbType type, object value)
        {
            // Validate use only DbType allow
            switch (type)
            {
                case  DbType.String:
                case  DbType.Boolean:
                case  DbType.Byte:
                case  DbType.Int16:
                case  DbType.Int32:
                case  DbType.Int64:
                case  DbType.Double:
                case  DbType.Decimal:
                case  DbType.DateTime:
                case  DbType.Date:
                    {
                        break;
                    }

                default:
                    {
                        // To: Developer, please only use DbType supported in Viewpoint Database Type
                        throw new NotSupportedException(type.ToString() + " not supported, please use DbType supported in Viewpoint Database Type");
                    }
            }

            // Validate use General .NET Framework Type
            if (value != null && !value.GetType().FullName.StartsWith("System."))
                // To: Developer, ENUM value is not accepted, require to use Conversion function like CInt to convert to General .NET Data Type
                throw new NotSupportedException(value.GetType().FullName + " not supported, please use Type Conversion extension function like Enum.ToEnumValue to pass in value");
        }

        /// <summary>
        ///  create command object and process parameters
        /// </summary>
        /// <returns>Returns SqlCommand</returns>
        private SqlCommand GetSqlCommand()
        {
            SqlCommand cmd = new SqlCommand(GetFinalQuery(), _dbConnection.Connection);
            cmd.Parameters.AddRange(_paramaters.ToArray());
            return cmd;
        }

    }
}
