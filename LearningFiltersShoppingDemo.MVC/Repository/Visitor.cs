using System.Linq.Expressions;
using System.Reflection;

namespace LearningFiltersShoppingDemo.MVC.Repository;

class Visitor : ExpressionVisitor
{
    protected override Expression VisitMember(MemberExpression memberExpression)
    {
        // Recurse down to see if we can simplify...
        var expression = Visit(memberExpression.Expression);

        // If we've ended up with a constant, and it's a property or a field,
        // we can simplify ourselves to a constant
        if (expression is ConstantExpression)
        {
            object container = ((ConstantExpression)expression).Value;
            var member = memberExpression.Member;
            if (member is FieldInfo)
            {
                object value = ((FieldInfo)member).GetValue(container);
                return Expression.Constant(value);
                //return value;
            }
            if (member is PropertyInfo)
            {
                object value = ((PropertyInfo)member).GetValue(container, null);
                return Expression.Constant(value);
            }
        }
        return base.VisitMember(memberExpression);
    }

    public object GetConstantValue(ConstantExpression constantExpression)
    {
        return constantExpression.Value;
    }
}

//class Visitor : ExpressionVisitor
//{

//    protected override Expression VisitBinary(BinaryExpression node)
//    {

//        var memberLeft = node.Left as MemberExpression;
//        if (memberLeft != null && memberLeft.Expression is ParameterExpression)
//        {

//            var f = Expression.Lambda(node.Right).Compile();
//            var value = f.DynamicInvoke();
//        }

//        return base.VisitBinary(node);
//    }
//}