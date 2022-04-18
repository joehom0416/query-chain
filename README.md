# query-chain
query-chain is a lightweight fuent api data access library that build on top of ADO.NET.


# Initialising connection
QueryConnection is the core component, you required this to create Query object.
```csharp

  SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.InitialCatalog = "database-name";
            builder.UserID = "sa";
            builder.Password = "12345";
          QueryConnection db = new QueryConnection(builder);
```

# Create Query Object 
The QueryConnection provided 2 functions to create Query object. `CreateQuery` and `CreateStoredProcedure`, which indicate query and stored procudure.

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
 string[] params = { "0001", "1001","1233", "8911" };
 _db.CreateQuery("SELECT * FROM Student WHERE StudentId IN(@StudentId)").AddParameters("StudentId", params, DbType.String);
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

# Result
The Query-Chain provided several return type.

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
           
IList<StudentModel> list2 = _db.CreateStoredProcedure("GetStudentList").GetCustomCollection<StudentModel>();
```

