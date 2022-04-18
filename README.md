# query-chain
query-chain is a lightweight fuent api data access library that build on top of ADO.NET.


# Initialising connection
QueryConnection is the core component, you required this to create Query object.
```c-sharp

  SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.InitialCatalog = "database-name";
            builder.UserID = "sa";
            builder.Password = "145837";
          QueryConnection db = new QueryConnection(builder);
```

# Create Query Object 
The QueryConnection provided 2 functions to create Query object. `CreateQuery` and `CreateStoredProcedure`, which indicate query and stored procudure.

## CreateQuery
```c-sharp
 _db.CreateQuery("SELECT * FROM Students WHERE StudentId=@StudentId")
```

## CreateStoredProcedure
```c-sharp
_db.CreateStoredProcedure("GetStudent")
```
