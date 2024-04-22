using Dapper;
using System.Data;
using System.Linq.Expressions;
using LearningFiltersShoppingDemo.MVC.Models;
using System.Linq;
using System;
using DapperExtensions;
using Dapper.SqlGenerator;
using System.Reflection;

namespace LearningFiltersShoppingDemo.MVC.Repository;

public class GenericRepo<T>
{
    private readonly IDbConnection _connection;

    public GenericRepo(IDbConnection connection)
    {
        _connection = connection;
    }

    public T Get(int id)
    {
        // Use Dapper's QueryFirstOrDefault method to execute the GetProduct stored procedure
        return _connection.QueryFirstOrDefault<T>("SP_GetProduct", new { ProductId = id }, commandType: CommandType.StoredProcedure);
    }

    public IQueryable<T> GetAll()
    {
        return _connection.Query<T>("SELECT * FROM " + typeof(T).Name).AsQueryable();
    }

    public void Insert(string name, decimal price, int categoryId)
    {
        // Use Dapper's Execute method to execute the InsertProduct stored procedure
        _connection.Execute("SP_InsertProduct", new { Name = name, Price = price, CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
    }

    public void Update(int id, string name, decimal price, int categoryId)
    {
        // Use Dapper's Execute method to execute the UpdateProduct stored procedure
        _connection.Execute("SP_UpdateProduct", new { ProductId = id, Name = name, Price = price, CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
    }

    public void Delete(int id)
    {
        // Use Dapper's Execute method to execute the DeleteProduct stored procedure
        _connection.Execute("SP_DeleteProduct", new { ProductId = id }, commandType: CommandType.StoredProcedure);
    }

    //public IEnumerable<T> GetPaged(int page, int pageSize)
    //{
    //    // Calculate offset based on page number and page size
    //    int offset = (page - 1) * pageSize;
    //    // Use Dapper's Query method to execute a paged query
    //    return _connection.Query<T>("SELECT * FROM Products ORDER BY ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
    //        new { Offset = offset, PageSize = pageSize });
    //}

    //public IQueryable<T> GetPaged(int page, int pageSize)
    //{
    //    // Calculate offset based on page number and page size
    //    int offset = (page - 1) * pageSize;

    //    // Use Dapper's Query method to execute a paged query
    //    var query = _connection.Query<T>("SELECT * FROM Products ORDER BY ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
    //        new { Offset = offset, PageSize = pageSize });

    //    // Convert the result to IQueryable
    //    return query.AsQueryable();
    //}

    // New method for efficient pagination

    public IQueryable<T> GetPaged(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;
        return GetAll().Skip(skip).Take(pageSize);
    }

    public IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter)
    {

        ////return GetAll().Where(filter);
        #region Method 1: Fetching all database first, then filtering. [works fine, but not with large databases because Dapper does not support IQueryable]
        //var query = _connection.Query<T>("SELECT * FROM " + typeof(T).Name);
        //var result = query.Where(filter.Compile());
        //return result.AsQueryable();
        #endregion
        ////return query.Where(filter.Compile()).AsQueryable();

        #region Method 2: Approach correct but facing errors [DapperExtensions Package]
        //var predicate = Predicates.FromExpression(filter);

        //// Construct the SQL query using DapperExtensions
        //var sqlQuery = new SqlBuilder().AddTemplate("SELECT * FROM " + typeof(T).Name + " WHERE {@Predicate}");
        //sqlQuery.Parameters.Add("Predicate", predicate);

        //// Execute the query using Dapper
        //var result = _connection.Query<T>(sqlQuery.RawSql, sqlQuery.Parameters);

        //// Return the result as an IQueryable
        //return result.AsQueryable();
        #endregion

        #region Method 3: Approach correct but facing errors [Dapper.SqlGenerator Package]
        //var sql = ConvertExpressionToSql(filter);
        //return _connection.Query<T>(sql).AsQueryable();
        #endregion

        #region Method 4: [Manual Approach] Writing logic, Since Dapper does not supports IQueryable directly, and other NuGet packages have some or the other issues
        var tableName = typeof(T).Name;
        var whereClause = filter != null ? $"WHERE {GetSqlWhereClause(filter)}" : "";
        var query = $"SELECT * FROM {tableName} {whereClause}";

        return _connection.Query<T>(query).AsQueryable();
        #endregion
    }

    #region Method 4 methods

    private string GetSqlWhereClause(Expression<Func<T, bool>> filter)
    {
        //Console.WriteLine($"{new Visitor().Visit(filter)}");
        #region Converting to expression vars to numbers
        //var filterCleaned = new Visitor().Visit(filter);
        #endregion

        //return GetSqlWhereClauseFromExpression(filterCleaned);
        return GetSqlWhereClauseFromExpression(filter.Body);


        //var entityRepository = new EntityRepository<T>(_connection);
        //var sqlWhereClause = entityRepository.GetSqlWhereClause(filter);

        //return sqlWhereClause;
    }

    private string GetSqlWhereClauseFromExpression(Expression expression)
    {
        if (expression is BinaryExpression binaryExpression)
        {
            var leftExpression = binaryExpression.Left;
            var left = GetSqlWhereClauseFromExpression(leftExpression);
            var rightExpression = binaryExpression.Right;
            var right = GetSqlWhereClauseFromExpression(rightExpression);
            var operatorType = binaryExpression.NodeType;
            var op = GetSqlOperator(operatorType);

            return $"({left} {op} {right})";
        }
        else if (expression is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Not)
        {
            var operand = GetSqlWhereClauseFromExpression(unaryExpression.Operand);
            return $"NOT ({operand})";
        }
        else if (expression is MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.Name == "Contains")
            {
                var member = (MemberExpression)methodCallExpression.Arguments[0];
                var propertyName = member.Member.Name;
                var value = ((ConstantExpression)methodCallExpression.Arguments[1]).Value;
                return $"{propertyName} LIKE '%{value}%'";
            }
            else if (methodCallExpression.Method.Name == "IsNullOrEmpty")
            {
                var member = (MemberExpression)methodCallExpression.Arguments[0];
                var propertyName = member.Member.Name;
                return $"{propertyName} IS NULL OR {propertyName} = ''";
            }
            else if (methodCallExpression.Method.Name == "Parse")
            {
                var member = (MemberExpression)methodCallExpression.Arguments[0];
                var propertyName = member.Member.Name;
                var value = ((ConstantExpression)methodCallExpression.Arguments[0]).Value; // Corrected index to 1
                return $"{propertyName} = {value}";
            }
        }
        else if (expression is MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression constantExpression)
            {
                // Handle expressions like value(LearningFiltersShoppingDemo.MVC.Controllers.ProductController+<>c__DisplayClass5_0).minPrice
                //var instance = constantExpression.Value;
                //var property = instance.GetType().GetProperty(memberExpression.Member.Name);
                //var value = property.GetValue(instance);
                //return value.ToString(); // Assuming value is not null

                //var value = new Visitor().Visit(memberExpression);

                //var value = new Visitor().GetConstantValue(constantExpression);
                //return value.ToString();

                object container = ((ConstantExpression)constantExpression).Value;
                var member = memberExpression.Member;
                if (member is FieldInfo)
                {
                    object value = ((FieldInfo)member).GetValue(container);
                    //return Expression.Constant(value);
                    return value.ToString();
                }
                if (member is PropertyInfo)
                {
                    object value = ((PropertyInfo)member).GetValue(container, null);
                    return value.ToString();
                }
            }
            else
            {
                // Handle normal member expressions like p.minPrice
                var propertyName = memberExpression.Member.Name;
                return propertyName;
            }
        }
        else if (expression is ConstantExpression constantExpression)
        {
            var value = constantExpression.Value;
            return value?.ToString() ?? "NULL"; // Return the constant value as string, handling nulls
        }

        throw new NotSupportedException($"Expression of type '{expression.GetType().Name}' is not supported.");
    }






    private string GetSqlOperator(ExpressionType nodeType)
    {
        switch (nodeType)
        {
            case ExpressionType.Equal: return "=";
            case ExpressionType.NotEqual: return "<>";
            case ExpressionType.GreaterThan: return ">";
            case ExpressionType.GreaterThanOrEqual: return ">=";
            case ExpressionType.LessThan: return "<";
            case ExpressionType.LessThanOrEqual: return "<=";
            case ExpressionType.AndAlso: return "AND";
            case ExpressionType.OrElse: return "OR";
            default: throw new NotSupportedException($"The operator '{nodeType}' is not supported.");
        }
    }

    #endregion

    #region Method 3
    //private string ConvertExpressionToSql(Expression<Func<T, bool>> filter)
    //{

    //    #region Method 3: Using Dapper.SqlGenerator [Not able to pass expressions directly]
    //    ////var generator = new Dapper.SqlGenerator.SqlGenerator<T>();
    //    var generator = _connection.Sql().SelectWhere<Products>(filter);
    //    var sql = generator.Select(filter);
    //    return sql;
    //    #endregion

    //    //var sql = filter.;
    //    //return sql;
    //}
    #endregion

    public IQueryable<T> GetPagedWithFilter(int page, int pageSize, string filter)
    {
        // Calculate offset based on page number and page size
        int offset = (page - 1) * pageSize;

        // Use Dapper's Query method to execute a paged query with a filter
        // Assuming 'filter' is a string that represents a WHERE clause condition
        var query = _connection.Query<T>($"SELECT * FROM {typeof(T).Name} WHERE {filter} ORDER BY ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
            new { Offset = offset, PageSize = pageSize });

        // Convert the result to IQueryable
        return query.AsQueryable();
    }
}
