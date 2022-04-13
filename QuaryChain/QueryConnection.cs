using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace QuaryChain
{
    /// <summary>
    /// SQL Database Connection
    /// </summary>
    public class QueryConnection
    {


        private readonly SqlConnection _dbConnection;

        public  SqlConnection Connection { get { return _dbConnection; } }

        public QueryConnection(SqlConnectionStringBuilder builder)
        {
            _dbConnection = new SqlConnection(builder.ConnectionString);
        }

        

        public Query CreateQuery(string query)
        {
            return new Query(_dbConnection,query);
        }


    }
}
