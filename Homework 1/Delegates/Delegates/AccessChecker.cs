using System.Reflection;

namespace Delegates;

internal static class AccessChecker
{
    internal static FieldInfo CheckField(FieldInfo? field, string fieldName)
    {
        if (field == null)
        {
            throw new FieldAccessException($"No such field {fieldName} in object");
        }

        if (field.IsPrivate)
        {
            throw new FieldAccessException($"Field {fieldName} is private in object");
        }

        if (!field.FieldType.IsArray)
        {
            throw new InvalidOperationException($"Field {fieldName} in object should be an array");
        }

        return field;
    }

    internal static bool CheckArray(object[] array, int arrayIndex, string arrayName)
    {
        if (!Enumerable.Range(0, array.Length).Contains(arrayIndex))
        {
            throw new IndexOutOfRangeException($"No such index {arrayIndex} in object field array " +
                                               $"{arrayName}");
        }

        return (array[arrayIndex]) != null;
    }
}