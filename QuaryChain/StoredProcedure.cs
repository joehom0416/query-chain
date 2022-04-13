using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace QuaryChain
{
    public class StoredProcedure
    {
        private readonly SqlConnection _dbConnection;
        private readonly string _query;
        private readonly List<SqlParameter> _paramaters;

    }
}
