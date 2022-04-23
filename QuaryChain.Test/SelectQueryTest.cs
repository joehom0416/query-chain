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
            //_db = new QueryConnection( "Server=127.0.0.1;Database=home-finance;User Id=sa;Password=145837;)";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.InitialCatalog = "home-finance";
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
                DbType.Guid,
                DbType.Date);
        }

        [Test]
        public void GetDataTable()
        {               
          DataTable dt=  _db
                .CreateQuery("SELECT * FROM Transactions WHERE Category=@category")
                .AddParameter("@category", "Beverage", DbType.String)
                .GetDataTable();  
          Assert.IsTrue(dt.Rows.Count> 0);
        }

        [Test]
        public void GetDataTable_ClearParams()
        {
            Query q = _db
                   .CreateQuery("SELECT * FROM Transactions WHERE Category=@category")
                    .AddParameter("@category", "Beverage", DbType.String);
            q.ClearParameter().AddParameter("@category", "Food", DbType.String);

            DataTable dt = q.GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [Test]
        public void GetDataTable_EnumParameter()
        {
            DataTable dt = _db
                  .CreateQuery("SELECT * FROM Transactions WHERE Category=@category")
                  .AddParameter("@category", Category.Bill.ToString())
                  .GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [Test]
        public void GetDataTableUsingIn()
        {

            DataTable dt = _db
                  .CreateQuery("SELECT * FROM Transactions WHERE Category IN (@category)")
                  .AddParameters("@category", new [] { "Bill", "Transport", "Food" }, DbType.String).GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }


        [Test]
        public void GetCustomCollection()
        {
            IList<TransactionModel> list = _db.CreateQuery("SELECT * FROM Transactions").GetCustomCollection<TransactionModel>();
            Assert.IsTrue(list.Count >0);

        }

        [Test]
        public async Task GetCustomCollectionAsync()
        {
            IList<TransactionModel> list = await _db.CreateQuery("SELECT * FROM Transactions").GetCustomCollectionAsync<TransactionModel>();
            Assert.IsTrue(list.Count > 0);

        }

        [Test]
        public void GetSingle()
        {
            TransactionModel r =  _db.CreateQuery("SELECT * FROM Transactions").GetSingle<TransactionModel>();
            Assert.IsTrue(r!=null);

        }

        [Test]
        public async Task GetSingleAsync()
        {
            TransactionModel r = await _db.CreateQuery("SELECT * FROM Transactions").GetSingleAsync<TransactionModel>();
            Assert.IsTrue(r != null);

        }
        [Test]
        public void ExecuteScalar()
        {
            int count = _db.CreateQuery("SELECT COUNT(*) FROM Transactions").ExecuteScalar<int>();
            Assert.IsTrue(count > 0);
        }
        [Test]
        public void UpdateSetatment()
        {
           int count= _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 95' WHERE Id=@Id")
                .AddParameter("@Id", new Guid("5072C3EE-8977-4113-883D-019530F6ACF0"), DbType.Guid).ExecuteNonQuery();
            Assert.IsTrue(count > 0);
        }

        [Test]
        public void TransactionRollback()
        {
             _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 95' WHERE Id=@Id")
                 .AddParameter("@Id", new Guid("5072C3EE-8977-4113-883D-019530F6ACF0"), DbType.Guid).ExecuteNonQuery();
            int count2 = _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 97' WHERE Id=@Id")
                 .AddParameter("@Id", new Guid("1f8a80d7-026f-4c6a-9845-151ccd2c6e84"), DbType.Guid).ExecuteNonQuery();
            _db.RollbackTransaction();
            Assert.IsTrue(count==1 && count2==1);
        }
        [Test]
        public void TransactionCommit()
        {
            _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 95' WHERE Id=@Id")
             .AddParameter("@Id", new Guid("5072C3EE-8977-4113-883D-019530F6ACF0"), DbType.Guid).ExecuteNonQuery();
            int count2 = _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 97' WHERE Id=@Id")
                 .AddParameter("@Id", new Guid("1f8a80d7-026f-4c6a-9845-151ccd2c6e84"), DbType.Guid).ExecuteNonQuery();
            _db.CommitTransaction();
            Assert.IsTrue(count == 1 && count2 == 1);
        }


        [Test]
        public async Task UpdateSetatmentAsync()
        {
            int count = await _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 95' WHERE Id=@Id")
                 .AddParameter("@Id", new Guid("5072C3EE-8977-4113-883D-019530F6ACF0"), DbType.Guid).ExecuteNonQueryAsync();
            Assert.IsTrue(count > 0);
        }
        [Test]
        public async Task TransactionCommitAsync()
        {
            _db.BeginTransaction();
            int count = await _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 95' WHERE Id=@Id")
           .AddParameter("@Id", new Guid("5072C3EE-8977-4113-883D-019530F6ACF0"), DbType.Guid).ExecuteNonQueryAsync();
            int count2 = await _db.CreateQuery("UPDATE Transactions SET [Description]='Petrol - 97' WHERE Id=@Id")
                 .AddParameter("@Id", new Guid("1f8a80d7-026f-4c6a-9845-151ccd2c6e84"), DbType.Guid).ExecuteNonQueryAsync();
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
            bool result =_db.HasRecords("Transactions", "1=1");
            Assert.IsTrue(result);
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
        Dictionary<string,dynamic>result=   _db.CreateStoredProcedure("NewTransaction")
                .AddParameter("@date", DateTime.Today)
                .AddParameter("@category", "Food")
                .AddParameter("@description", "Kajang Satay")
                .AddParameter("@price",(decimal)50.10)
                .AddOutputParameter("@newId", DbType.Guid)
                .ExecuteProcedure();

            Assert.IsTrue(! String.IsNullOrEmpty(result["@newId"].ToString()));
        }

        //[Test]
        //public void CallStoredProcedureWithNonQuery_ReturnValue()
        //{
        //    Dictionary<string, dynamic> result = _db.CreateStoredProcedure("SpDocNumGet")
        //            .AddParameter("@entCode", "GENERAL")
        //            .AddParameter("@dType", "CPA")
        //            .AddParameter("@mustbeNumeric", true)
        //            .AddReturnValueParameter("@return", DbType.Int64).ExecuteProcedure();

        //    Assert.IsTrue(((Int64)result["@return"]>0));
        //}

        [Test]
        public void CallStoredProcedureWithDataTable()
        {
            DataTable dt = _db.CreateStoredProcedure("GetTransactionFromMonthAndYear")
                .AddParameter("@year", 2022)
                .AddParameter("@month",4).GetDataTable();
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        private class TransactionModel
        {
            public Guid Id { get; set; }
            public DateTime? Date { get; set; }
            public string? Category { get; set; }
            public string? Description { get; set; }

            public Decimal? Price { get; set; }
        }

        private enum Category
        {
           Bill=1
        }
    }
}