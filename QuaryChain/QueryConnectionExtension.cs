using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuaryChain
{
    public static class QueryConnectionExtension
    {
        /// <summary>
        /// Table has records meeting the where criteria
        /// </summary>
        /// <param name="db">query connection</param>
        /// <param name="tableName">table name</param>
        /// <param name="sqlCondition">where condition</param>
        /// <returns>returns true if has records</returns>
        public static bool HasRecords(this QueryConnection db, string tableName, string sqlCondition)
        {
            string query = $"IF EXISTS( SELECT *  FROM   {tableName} WHERE  {sqlCondition}) SELECT 1 ELSE SELECT 0";
            return db.CreateQuery(query).ExecuteScalar<int>()==1;
        }

        /// <summary>
        ///  Gets database value
        /// </summary>
        /// <param name="db">query connection</param>
        /// <param name="field">field name</param>
        /// <param name="tableName">table name</param>
        /// <param name="sqlCondition">where condition</param>
        /// <returns>returns value</returns>
        public static object GetDbValue(this QueryConnection db, string field, string tableName, string sqlCondition)
        {
            string query = $"SELECT  {field} FROM {tableName} WHERE {sqlCondition}";
            return db.CreateQuery(query).ExecuteScalar();
        }
        /// <summary>
        ///  schema information for the data source of this SqlConnection. For more information about scheme
        /// </summary>
        /// <param name="db">query connection</param>
        /// <param name="collectionName">specifies the name of the schema to return.</param>
        /// <param name="restrictionValues">a set of restriction values for the requested schema.</param>
        /// <returns>returns A DataTable that contains schema information</returns>
        public static DataTable GetScheme(this QueryConnection db,string collectionName, string[] restrictionValues)
        {
            db.Open();
            DataTable result = db.Connection.GetSchema(collectionName, restrictionValues);
            db.Close();
            return result;
        }
    }
}
