using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CoffeeProductionApp.DAL
{
    public class Repository<T> : IRepository<T> where T : new()
    {
        private readonly string _tableName;
        private readonly PropertyInfo[] _properties;

        public Repository()
        {
            _tableName = EntityMapper.GetTableName<T>();
            _properties = typeof(T).GetProperties();
        }

        public DataTable GetAll()
        {
            string query = $"SELECT * FROM {_tableName}";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public T GetById(int id)
        {
            string idColumnName = GetIdColumnName();
            string query = $"SELECT * FROM {_tableName} WHERE {idColumnName} = @id";
            SqlParameter[] parameters = { new SqlParameter("@id", id) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0)
                return default(T);

            return EntityMapper.MapToEntity<T>(dt.Rows[0]);
        }

        public int Add(T entity)
        {
            var (columns, values, parameters) = BuildInsertParameters(entity);
            string query = $@"
                INSERT INTO {_tableName} ({columns})
                VALUES ({values});
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                    return 0;
                }
            }
        }

        public int Update(T entity)
        {
            var (setClause, parameters) = BuildUpdateParameters(entity);
            int id = GetIdValue(entity);
            string idColumnName = GetIdColumnName();

            string query = $@"
                UPDATE {_tableName} 
                SET {setClause}
                WHERE {idColumnName} = @id";

            SqlParameter[] allParams = new SqlParameter[parameters.Length + 1];
            Array.Copy(parameters, allParams, parameters.Length);
            allParams[parameters.Length] = new SqlParameter("@id", id);

            return DatabaseHelper.ExecuteNonQuery(query, allParams);
        }

        public int Delete(int id)
        {
            string idColumnName = GetIdColumnName();
            string query = $"DELETE FROM {_tableName} WHERE {idColumnName} = @id";
            SqlParameter[] parameters = { new SqlParameter("@id", id) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public DataTable ExecuteCustomQuery(string query, SqlParameter[] parameters = null)
        {
            return DatabaseHelper.ExecuteQuery(query, parameters);
        }

        public int ExecuteCustomCommand(string query, SqlParameter[] parameters = null)
        {
            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        private string GetIdColumnName()
        {
            string query = $"SELECT TOP 1 * FROM {_tableName}";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName.ToLower();
                if (colName == "ид" || colName.StartsWith("ид_"))
                {
                    return col.ColumnName;
                }
            }

            return dt.Columns[0].ColumnName;
        }

        private (string columns, string values, SqlParameter[] parameters) BuildInsertParameters(T entity)
        {
            List<string> columnList = new List<string>();
            List<string> valueList = new List<string>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            foreach (PropertyInfo prop in _properties)
            {
                if (prop.Name == "Id") continue;

                object value = prop.GetValue(entity);
                if (value == null) continue;

                string columnName = EntityMapper.GetColumnName(prop.Name);
                string paramName = "@" + prop.Name;

                columnList.Add(columnName);
                valueList.Add(paramName);
                paramList.Add(new SqlParameter(paramName, value ?? DBNull.Value));
            }

            return (
                string.Join(", ", columnList),
                string.Join(", ", valueList),
                paramList.ToArray()
            );
        }

        private (string setClause, SqlParameter[] parameters) BuildUpdateParameters(T entity)
        {
            List<string> setList = new List<string>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            foreach (PropertyInfo prop in _properties)
            {
                if (prop.Name == "Id") continue;

                object value = prop.GetValue(entity);
                string columnName = EntityMapper.GetColumnName(prop.Name);
                string paramName = "@" + prop.Name;

                setList.Add($"{columnName} = {paramName}");
                paramList.Add(new SqlParameter(paramName, value ?? DBNull.Value));
            }

            return (
                string.Join(", ", setList),
                paramList.ToArray()
            );
        }

        private int GetIdValue(T entity)
        {
            foreach (PropertyInfo prop in _properties)
            {
                if (prop.Name == "Id")
                {
                    return Convert.ToInt32(prop.GetValue(entity));
                }
            }
            throw new Exception("Id property not found");
        }
    }
}