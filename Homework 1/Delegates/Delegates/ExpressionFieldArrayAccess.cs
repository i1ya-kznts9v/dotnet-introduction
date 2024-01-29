using System.Linq.Expressions;
using System.Reflection;

namespace Delegates;

public static class ExpressionFieldArrayAccess
{
    public static Func<T, object?> FieldArrayAccess<T>(string query)
    {
        List<Tuple<string, int>> parsedQuery = QueryParser.ParseQuery(query);
        if (parsedQuery.Count == 0)
            throw new ArgumentException("Empty query for given object");

        ParameterExpression parameter = Expression.Parameter(typeof(T));
        Expression members = parameter;
        ConstantExpression nullConstant = Expression.Constant(null, typeof(object));
        LabelTarget returnLabel = Expression.Label(typeof(object));

        List<Expression> expressions = new List<Expression>();

        foreach (var part in parsedQuery)
        {
            string currentField = part.Item1;
            int currentIndex = part.Item2;

            AccessChecker.CheckField(
                members.Type.GetField(currentField,
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic),
                currentField);

            members = Expression.Field(members, currentField);
            members = Expression.ArrayAccess(members,
                Expression.Constant(currentIndex, typeof(int)));

            Expression member = Expression.IfThen(
                Expression.Equal(members, nullConstant),
                Expression.Return(returnLabel, nullConstant));

            expressions.Add(member);
        }

        expressions.Add(Expression.Label(returnLabel, members));

        BlockExpression block = Expression.Block(expressions);
        Expression<Func<T, object?>> lambda = Expression.Lambda<Func<T, object?>>(block, parameter);
        return lambda.Compile();
    }
}