using NUnit.Framework;
using QuaryChain;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        public void UpdateSetatment()
        {
           int count= _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                .AddParameter("@DbId", "DB01").ExecuteQuery();
            Assert.IsTrue(count > 0);
        }

        [Test]
        public void TransactionRollback()
        {
             _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB02 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteQuery();
            int count2 = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB02 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DEPLOY").ExecuteQuery();
            _db.RollbackTransaction();
            Assert.IsTrue(count==1 && count2==1);
        }
        [Test]
        public void TransactionCommit()
        {
            _db.BeginTransaction();
            int count = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteQuery();
            int count2 = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DEPLOY").ExecuteQuery();
            _db.RollbackTransaction();
            Assert.IsTrue(count == 1 && count2 == 1);
        }

        private class ClpDatabases
        {
            public string DbId { get; set; }
            public string Description { get; set; }
            public string DbType { get; set; }
            public string ServerName { get; set; }

            public int port { get; set; }
        }

    }
}