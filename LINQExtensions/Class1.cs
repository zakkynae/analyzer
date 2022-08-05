namespace LINQExtensions;
public static class EnumerableExtensions
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
    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => (enumerable == null || enumerable.Count() == 0);

    public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T item)
    {
        var list = new List<T>(enumerable);
        list.Insert(index, item);
        return list;
    }

    public static IEnumerable<T> Disorder<T>(this IEnumerable<T> enumerable)
    {
        var rnd = new Random();
        return enumerable.OrderBy(x => rnd.Next());
    }
    //public static IEnumerable<T> Split<T>(this IEnumerable<T> enumerable, T item)
    //{
    //    return enumerable.ToString().Split((char)item).ToList();
    //}


}
