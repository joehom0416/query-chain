# query-chain
query-chain is a lightweight data access library for SQL server, it is based on a Fluent API design pattern ( a.k.a Fluent Interface) where the result is formulated by method chaining.

# Install
Nuget package
```
Install-Package QuaryChain -Version 1.0.0
```

# Initialising connection
QueryConnection is the core component, you required this to create Query object.
```csharp

  SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.InitialCatalog = "database-name";
            builder.UserID = "sa";
            builder.Password = "12345";
          QueryConnection db = new QueryConnection(builder);
          
         QueryConnection db = new QueryConnection("Server=127.0.0.1;Database=db-2;User Id=sa;Password=12345;"); 
```

# Restrict DbType used in your database
In some scenarios, you need to restrict your developers to use some DbTypes that your database supported, you can use `SetSupportedDbType()`.
```csharp

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
```

# Create Query Object 
The QueryConnection provided 2 functions to create Query object. `CreateQuery()` and `CreateStoredProcedure()`, which indicate query and stored procudure.

### CreateQuery
```csharp
 _db.CreateQuery("SELECT * FROM Students WHERE StudentId=@StudentId")
```

### CreateStoredProcedure
```csharp
_db.CreateStoredProcedure("GetStudent");
```

# Add Parameter
`Query` provided several API for add parameter.
### AddParameter
Add Input Direction Parameter.
```csharp
 _db.CreateQuery("SELECT * FROM Students WHERE StudentId=@StudentId")
                .AddParameter("@StudentId", "0001");
                
  _db.CreateStoredProcedure("GetStudent")
                .AddParameter("@StudentId", "0001", DbType.String)
                .AddParameter("@Course", "DIT", DbType.String);

```

### AddParameters
This function provided dynamic parameters for SQL IN Operator.
```csharp
 _db.CreateQuery("SELECT * FROM Student WHERE StudentId IN(@StudentId)")
 .AddParameters("StudentId", new [] {"0001", "1001","1233", "8911"}, DbType.String);
```

### AddReturnValueParameter
Add ReturnValue Direction Parameter.
```csharp
_db.CreateStoredProcedure("GetNewStudentId").AddReturnValueParameter("@return", DbType.String)
```

### AddOutputParameter
Add Output Direction Parameter.
```csharp
_db.CreateStoredProcedure("GetCourseExamId").AddOutputParameter("@output", DbType.String)
```

### ClearParameter
Clear parameters
```csharp

Query q = _db.CreateQuery("SELECT * FROM ClpDatabases WHERE DbId=@DbId")
                   .AddParameter("@DbId", "DB01", DbType.String);
                   
            q.ClearParameter().AddParameter("@DbId", "LOCAL", DbType.String); ;
```

# SQL Result
The Query-Chain provided several methods to get different result from sql server database.

### GetDataTable
Returns DataTable
```csharp

DataTable dt=  _db
                .CreateQuery("SELECT * FROM Students WHERE Year=@Year")
                .AddParameter("@Year", 2021, DbType.Int)
                .GetDataTable();  

 DataTable dt = _db.CreateStoredProcedure("GetStudentList").AddParameter("@Year", 2021).GetDataTable();
```


### GetCustomCollection and GetCustomCollectionAsync
Returns list of custom models
```csharp

IList<StudentModel> list = _db.CreateQuery("SELECT * FROM Students").GetCustomCollection<StudentModel>();
IList<StudentModel> list3 = await _db.CreateQuery("SELECT * FROM Students").GetCustomCollectionAsync<StudentModel>();
           
IList<StudentModel> list2 = _db.CreateStoredProcedure("GetStudentList").GetCustomCollection<StudentModel>();
IList<StudentModel> list4 = await _db.CreateStoredProcedure("GetStudentList").GetCustomCollectionAsync<StudentModel>();
```

### GetSingle and GetSingleAsync
Returns a custom models
```csharp

StudentModel result = _db.CreateQuery("SELECT * FROM Students WHERE StudentId=@StudentId").AddParameter("@StudentId", "0001").GetSingle<StudentModel>();
StudentModel result2  = await _db.CreateQuery("SELECT * FROM Students WHERE StudentId=@StudentId").AddParameter("@StudentId", "0001").GetSingleAsync<StudentModel>();
           

```
### ExecuteScalar
```csharp
 int count = _db.CreateQuery("SELECT COUNT(*) FROM ClpDatabases").ExecuteScalar<int>();
```

### ExecuteNonQuery and ExecuteNonQueryAsync
```csharp
  int recordAffected= _db.CreateQuery("UPDATE Courses SET [Description]='Bachelor of Arts' WHERE Id=@Id")
                .AddParameter("@Id", "C01").ExecuteNonQuery();

int recordAffected1= await _db.CreateQuery("UPDATE Courses SET [Description]='Bachelor of Arts' WHERE Id=@Id")
                .AddParameter("@Id", "C01").ExecuteNonQueryAsync();
```
### ExecuteProcedure
Returns a dictionary consist of returnValue and output.
```csharp
 Dictionary<string,dynamic> result = _db.CreateStoredProcedure("GetTutorialClass")
                .AddParameter("@course", "DIT")
                .AddOutputParameter("@output", DbType.String)
                .ExecuteProcedure();
                
    Dictionary<string, dynamic> result = _db.CreateStoredProcedure("GetRunningNumber")
                    .AddReturnValueParameter("@return", DbType.Int64).ExecuteProcedure();             
                
```

### Async with Cancellation Token
you can pass in the cancellation token to all async methods.
```csharp
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
```


# Local Transaction
Work as Transaction in ADO.Net, used to bind multiple tasks together so that execute as a single unit of work. It similar to ADO.Net, you required to call `BeginTransaction()` method from QueryConnection Object. Once you have begun a transaction, you can perform any execution and called `CommitTransaction()` to commit your sql command to database or call `RollbackTransaction()` to Rollback your sql command. 
```csharp
   _db.BeginTransaction();
            try
            {
                int count = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                 .AddParameter("@DbId", "DB01").ExecuteNonQuery();
                int count2 = _db.CreateQuery("UPDATE ClpDatabases SET [Description]='DB01 -test' WHERE DbId=@DbId")
                     .AddParameter("@DbId", "DEPLOY").ExecuteNonQuery();
                _db.CommitTransaction();
            }
            catch(SqlException ex)
            {
                _db.RollbackTransaction();
            }

```
