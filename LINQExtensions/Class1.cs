using System.Linq;
namespace LINQExtensions;
public static class IEnumerableExtensions
{
    public static IEnumerable<T> Ensure<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        foreach (T item in enumerable)
        {
            if (!predicate(item)) throw new ArgumentException(); 
            else yield return item;
        }        
    }
    public static IEnumerable<T> EnshureCount<T>(this IEnumerable<T> enumerable, Func<int, bool> predicate)
    {
        var count = 0;
        foreach (T item in enumerable)
        {   
            count++;
            yield return item;
        }
        if (!predicate(count)) throw new ArgumentException();
    }
    public static bool Any<T>(this IEnumerable<T> enumerable, int count, Func<T, Boolean> predicate)
    {
        var index = 0;
        foreach(T item in enumerable)
            if(predicate(item)) index++;
        return index == count;
    }
    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => (enumerable.Count() == 0 || enumerable == null) ? true : false;
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerable, int length)
    {
        var values = new List<IEnumerable<T>>();

    }

}
