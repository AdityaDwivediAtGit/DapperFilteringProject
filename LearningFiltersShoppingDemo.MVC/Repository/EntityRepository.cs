using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class EntityRepository<T>
{
    private readonly string _connectionString;

    public EntityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<T> GetEntities<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, bool descending)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = filter.Body;

        var propertyValues = new Dictionary<string, object>();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var propertyName = property.Name;
            var propertyValue = GetPropertyValue<object>(body, propertyName);

            if (propertyValue != null)
            {
                propertyValues.Add(propertyName, propertyValue);
            }
        }

        var sqlQuery = $"SELECT * FROM [{typeof(T).Name}] AS [e] WHERE ";

        var conditions = new List<string>();

        foreach (var property in propertyValues)
        {
            var propertyValue = property.Value;
            var propertyType = propertyValue.GetType();
            var sqlType = GetSqlType(propertyType);

            if (propertyType == typeof(int))
            {
                conditions.Add($"[e].[{property.Key}] = {propertyValue}");
            }
            else if (propertyType == typeof(string))
            {
                conditions.Add($"[e].[{property.Key}] = '{propertyValue}'");
            }
            else if (propertyType == typeof(decimal))
            {
                conditions.Add($"[e].[{property.Key}] = {propertyValue}");
            }
            else
            {
                throw new NotSupportedException($"The property type '{propertyType.Name}' is not supported.");
            }
        }

        sqlQuery += string.Join(" AND ", conditions);

        if (orderBy != null)
        {
            sqlQuery += $" ORDER BY [e].[{GetPropertyValue<string>(orderBy.Body, orderBy.Parameters[0].Name)}] {(descending ? "DESC" : "ASC")}";
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                var reader = command.ExecuteReader();
                var entities = new List<T>();

                while (reader.Read())
                {
                    var entity = Activator.CreateInstance<T>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var columnValue = reader[i];

                        if (columnValue != DBNull.Value)
                        {
                            var property = typeof(T).GetProperty(columnName);
                            var propertyType = property.PropertyType;
                            var convertedValue = Convert.ChangeType(columnValue, propertyType);

                            property.SetValue(entity, convertedValue);
                        }
                    }

                    entities.Add(entity);
                }

                return entities;
            }
        }
    }

    private T GetPropertyValue<T>(Expression expression, string propertyName)
    {
        var memberExpression = (MemberExpression)expression;
        var propertyInfo = (PropertyInfo)memberExpression.Member;
        var value = propertyInfo.GetValue(((ConstantExpression)memberExpression.Expression).Value);

        return (T)value;
    }

    private string GetSqlType(Type type)
    {
        if (type == typeof(int))
        {
            return "int";
        }
        else if (type == typeof(string))
        {
            return "nvarchar(max)";
        }
        else if (type == typeof(decimal))
        {
            return "decimal(18,2)";
        }
        else
        {
            throw new NotSupportedException($"The property type '{type.Name}' is not supported.");
        }
    }
}