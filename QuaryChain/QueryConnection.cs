using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;

namespace QuaryChain
{
    /// <summary>
    /// SQL Database Connection
    /// </summary>
    public class QueryConnection
    {


        private readonly SqlConnection _dbConnection;
        private  SqlTransaction _transaction;
        private  bool _transactionMode;
        public  SqlConnection Connection { get { return _dbConnection; } }
        public QueryConnection(SqlConnectionStringBuilder builder)
        {
            _dbConnection = new SqlConnection(builder.ConnectionString);
        }

        public QueryConnection BeginTransaction()
        {
            _transactionMode = true;
            return this;
        }

        public QueryConnection CommitTransaction()
        {
            _transactionMode = false;
            _transaction.Commit();
            Close();
            return this;
        }
        
        public QueryConnection RollbackTransaction()
        {
            _transactionMode = false;
            _transaction.Rollback();
            Close();
            return this;
        }
      
       
        /// <summary>
        /// Open Connection 
        /// </summary>
        /// <param name="cmd">sql command object</param>
        public void Open(SqlCommand cmd)
        {

            if(_dbConnection.State!= ConnectionState.Open) 
                _dbConnection.Open(); 

            if(cmd!=null && _transactionMode )
            {
                if(_transaction == null)
                {
                    _transaction = _dbConnection.BeginTransaction(Guid.NewGuid().ToString().Substring(0, 31));
                }
               
                cmd.Transaction = _transaction;
            }
           
        }
        /// <summary>
        /// Open Connection 
        /// </summary>
        public void Open()
        {
            Open(null);
        }


        /// <summary>
        /// Open database connection with asynchronous method
        /// </summary>
        /// <param name="cmd">command object</param>
        public async Task OpenAsync(SqlCommand cmd, CancellationToken cancellationToken)
        {
            if (_dbConnection.State != ConnectionState.Open)
                await _dbConnection.OpenAsync(cancellationToken);
            
            if (cmd!= null && _transactionMode)
            {
                if (_transaction == null)
                {
                    _transaction = await Task.Run<SqlTransaction>(() => _dbConnection.BeginTransaction(Guid.NewGuid().ToString().Substring(0,31)));
                }
                cmd.Transaction = _transaction;
            }
        }
        public async Task OpenAsync(SqlCommand cmd)
        {
            await OpenAsync(cmd, CancellationToken.None);
        }

        public async Task OpenAsync()
        {
            await OpenAsync(null);
        }

        /// <summary>
        /// Close Connection
        /// </summary>
        public void Close()
        {
            if (!_transactionMode)
            {
                _dbConnection.Close();
                _transaction = null;
            }
           
        }
        /// <summary>
        /// Create Query object
        /// </summary>
        /// <param name="query">sql query, stored procedure</param>
        /// <returns></returns>
        public Query CreateQuery(string query)
        {
            return new Query(this,query);
        }


    }
}
