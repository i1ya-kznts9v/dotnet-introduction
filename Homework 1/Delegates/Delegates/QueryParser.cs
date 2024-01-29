namespace Delegates;

public static class QueryParser
{
    public static List<Tuple<string, int>> ParseQuery(string query)
    {
        return (from part in query.Split('.')
            let bracket = part.IndexOf('[')
            let field = part[..bracket]
            let index = part.Substring(bracket + 1, part.Length - 2 - bracket)
            select new Tuple<string, int>(field, int.Parse(index))).ToList();
    }
}