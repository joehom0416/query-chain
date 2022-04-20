using NUnit.Framework;
using QuaryChain.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace QuaryChain.Test
{
    public class Tests
    {
        QueryConnection _db;
       [SetUp]
        public void Setup()
        {
            // string conn = "Server=127.0.0.1;Database=VP80xx;User Id=sa;Password=145837;";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.InitialCatalog = "VP80xx";
            builder.UserID = "sa";
            builder.Password = "145837";
            _db = new QueryConnection(builder);
            _db.SetSupportedDbType(DbType.String,
                DbType.Boolean,
                DbType.Byte,
                DbType.Int16,
                DbType.Int32,
                DbType.Int64,
                DbType.Double,
                DbType.Decimal,
                DbType.DateTime,
                DbType.Date);
        }

        [Test]
        public void GetDataTable()
        {               
          DataTable dt=  _db
                .CreateQuery("SELECT * FROM ClpDatabases WHERE DbId=@DbId")
                .AddParameter("@DbId", "DB01", DbType.String)
                .GetDataTable();  
          Assert.IsTrue(dt.Rows.Count> 0);
        }

        [Test]
        public void GetDataTable_ClearParams()
        {
            Query q = _db
                   .CreateQuery("SELECT * FROM ClpDatabases WHERE DbId=@DbId")
                   .AddParameter("@DbId", "DB01", DbType.String);
            q.ClearParameter().AddParameter("@DbId", "LOCAL", DbType.String); ;

            DataTable dt = q.GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [Test]
        public void GetDataTable_EnumParameter()
        {
            DataTable dt = _db
                  .CreateQuery("SELECT * FROM ClpDatabases WHERE RecStatus=@RecStatus")
                  .AddParameter("@RecStatus", (int)RecStatus.Active)
                  .GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [Test]
        public void GetDataTableUsingIn()
        {
            string[] param = { "DB01", "LOCAL" };
            DataTable dt = _db
                  .CreateQuery("SELECT * FROM ClpDatabases WHERE DbId IN(@DbId)")
                  .AddParameters("@DbId", param, DbType.String).GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }


        [Test]
        public void GetCustomCollection()
        {
            IList<ClpDatabases> list = _db.CreateQuery("SELECT * FROM ClpDatabases").GetCustomCollection<ClpDatabases>();
            Assert.IsTrue(list.Count >0);

        }

        [Test]
        public async Task GetCustomCollectionAsync()
        {
            IList<ClpDatabases> list = await _db.CreateQuery("SELECT * FROM ClpDatabases").GetCustomCollectionAsync<ClpDatabases>();
            Assert.IsTrue(list.Count > 0);

        }

        [Test]
        public void ExecuteScalar()
        {
            int count = _db.CreateQuery("SELECT COUNT(*) FROM ClpDatabases").ExecuteScalar<int>();
            Assert.IsTrue(count > 0);
        }
        [Test]
        public void UpdateSetatment()
        {
           int count= _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                .AddParameter("@DbId", "DB01").ExecuteNonQuery();
            Assert.IsTrue(count > 0);
        }

        [Test]
        public void TransactionRollback()
        {
             _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB02 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteNonQuery();
            int count2 = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB02 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DEPLOY").ExecuteNonQuery();
            _db.RollbackTransaction();
            Assert.IsTrue(count==1 && count2==1);
        }
        [Test]
        public void TransactionCommit()
        {
            _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteNonQuery();
            int count2 = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DEPLOY").ExecuteNonQuery();
            _db.RollbackTransaction();
            Assert.IsTrue(count == 1 && count2 == 1);
        }


        [Test]
        public async Task UpdateSetatmentAsync()
        {
            int count = await _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB02 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteNonQueryAsync();
            Assert.IsTrue(count > 0);
        }
        [Test]
        public async Task TransactionCommitAsync()
        {
            _db.BeginTransaction();
            int count = await _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteNonQueryAsync();
            int count2 = await _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DEPLOY").ExecuteNonQueryAsync();
            _db.RollbackTransaction();
            Assert.IsTrue(count == 1 && count2 == 1);
        }

        [Test]
        public async Task CancelToken()
        {
            CancellationTokenSource source  = new CancellationTokenSource();
            source.CancelAfter(2000);// give up after 2 seconds
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                await _db.CreateQuery("WAITFOR DELAY '00:00:30'").ExecuteNonQueryAsync(source.Token);
            }catch(System.Data.SqlClient.SqlException ex)
            {
                // cancellation will throw exception, catch it here
            }
            finally
            {
                stopWatch.Stop();
            }
           
            
            TimeSpan ts =stopWatch.Elapsed;
            Assert.IsTrue(ts.Seconds<30);
        }


        [Test]
        public void HasRecords()
        {
            bool result =_db.HasRecords("ClpDatabases", "1=1");
            Assert.IsTrue(result);
        }

        [Test]
        public void GetDbValue()
        {
            string result = (string) _db.GetDbValue("DbType","ClpDatabases", "DbId='DB01'");
            Assert.IsTrue(result=="L");
        }

        [Test]
        public void GetScheme()
        {
            DataTable result = _db.GetScheme("Tables", null);
            Assert.IsTrue(result.Rows.Count>0);
        }

        [Test]
        public void CallStoredProcedureWithNonQuery_ReturnOutput()
        {
        Dictionary<string,dynamic>result=   _db.CreateStoredProcedure("SpClpRootGroupGet")
                .AddParameter("@orgCode", "ViewPoint")
                .AddOutputParameter("@output", DbType.String)
                .AddParameter("@userCode", "sm").ExecuteProcedure();

            Assert.IsTrue(! String.IsNullOrEmpty(result["@output"].ToString()));
        }

        [Test]
        public void CallStoredProcedureWithNonQuery_ReturnValue()
        {
            Dictionary<string, dynamic> result = _db.CreateStoredProcedure("SpDocNumGet")
                    .AddParameter("@entCode", "GENERAL")
                    .AddParameter("@dType", "CPA")
                    .AddParameter("@mustbeNumeric", true)
                    .AddReturnValueParameter("@return", DbType.Int64).ExecuteProcedure();

            Assert.IsTrue(((Int64)result["@return"]>0));
        }

        [Test]
        public void CallStoredProcedureWithDataTable()
        {
            DataTable dt = _db.CreateStoredProcedure("SpClpMenusGet").AddParameter("@UserRole", "WM").GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        private class ClpDatabases
        {
            public string DbId { get; set; }
            public string Description { get; set; }
            public string DbType { get; set; }
            public string ServerName { get; set; }

            public int port { get; set; }
        }

        private enum RecStatus
        {
            Inactive=0,
            Active=1
        }
    }
}