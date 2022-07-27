using System.Linq;
namespace LINQExtensions;
public static class LinqExtensions 
{
    public static bool Ensure<T>(this IEnumerable<T> enumerable, Predicate<T> method)
    {
        foreach (T item in enumerable)
            if (!method(item)) throw new NotImplementedException();
        return true;
    }
}
