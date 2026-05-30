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
        private string _idColumnName;

        public Repository()
        {
            _tableName = EntityMapper.GetTableName<T>();
            _properties = typeof(T).GetProperties();
            _idColumnName = GetIdColumnName();
        }

        public DataTable GetAll()
        {
            string query = $"SELECT * FROM {_tableName}";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public T GetById(int id)
        {
            try
            {
                string query = $"SELECT * FROM {_tableName} WHERE {_idColumnName} = @id";
                SqlParameter[] parameters = { new SqlParameter("@id", id) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"GetById: Запись с ID {id} не найдена в таблице {_tableName}");
                    return default(T);
                }

                return EntityMapper.MapToEntity<T>(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetById ошибка: {ex.Message}");
                return default(T);
            }
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

            string query = $@"
                UPDATE {_tableName} 
                SET {setClause}
                WHERE {_idColumnName} = @id";

            SqlParameter[] allParams = new SqlParameter[parameters.Length + 1];
            Array.Copy(parameters, allParams, parameters.Length);
            allParams[parameters.Length] = new SqlParameter("@id", id);

            int result = DatabaseHelper.ExecuteNonQuery(query, allParams);
            System.Diagnostics.Debug.WriteLine($"Update: {query}, ID={id}, Result={result}");
            return result;
        }

        public int Delete(int id)
        {
            string query = $"DELETE FROM {_tableName} WHERE {_idColumnName} = @id";
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
            try
            {
                string query = $"SELECT TOP 1 * FROM {_tableName}";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                if (dt.Columns.Count == 0)
                {
                    return "ид";
                }

                foreach (DataColumn col in dt.Columns)
                {
                    string colName = col.ColumnName.ToLower();
                    if (colName == "ид" || colName.StartsWith("ид_"))
                    {
                        System.Diagnostics.Debug.WriteLine($"GetIdColumnName: Найдена колонка {col.ColumnName} для таблицы {_tableName}");
                        return col.ColumnName;
                    }
                }

                string firstColumn = dt.Columns[0].ColumnName;
                System.Diagnostics.Debug.WriteLine($"GetIdColumnName: Использую первую колонку {firstColumn} для таблицы {_tableName}");
                return firstColumn;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetIdColumnName ошибка: {ex.Message}");
                return "ид";
            }
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
                    object value = prop.GetValue(entity);
                    if (value != null)
                    {
                        return Convert.ToInt32(value);
                    }
                }
            }
            throw new Exception("Id property not found");
        }
    }
}