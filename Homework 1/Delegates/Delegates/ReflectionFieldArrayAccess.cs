using System.Reflection;

namespace Delegates;

public static class ReflectionFieldArrayAccess
{
    public static Func<T, object?> FieldArrayAccess<T>(string query)
    {
        return objectT =>
        {
            if (objectT == null)
                throw new ArgumentException("Given object for query is null");

            List<Tuple<string, int>> parsedQuery = QueryParser.ParseQuery(query);
            if (parsedQuery.Count == 0)
                throw new ArgumentException("Empty query for given object");

            object currentObject = objectT;
            foreach (var part in parsedQuery)
            {
                string currentField = part.Item1;
                int currentIndex = part.Item2;

                FieldInfo? field = currentObject.GetType().GetField(currentField,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                field = AccessChecker.CheckField(field, currentField);

                object[] currentFieldArray = (object[]) field.GetValue(currentObject)!;
                if (!AccessChecker.CheckArray(currentFieldArray, currentIndex, currentField))
                    return null;

                currentObject = currentFieldArray[currentIndex];
            }

            return currentObject;
        };
    }
}