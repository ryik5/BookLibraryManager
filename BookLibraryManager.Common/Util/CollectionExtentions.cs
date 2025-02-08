using System.Collections.ObjectModel;

namespace BookLibraryManager.Common.Util;

/// <author>YR 2025-02-06</author>
internal static class CollectionExtentions
{
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        if (collection is null || !collection.Any()) return;

        foreach (var t in collection)
            action(t);
    }


    public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> condition)
    {
        if (collection?.Any() != true) return;

        collection.Where(condition).ToList().ForEach(e => collection.Remove(e));
    }

    /// <summary>
    /// delete items from Collection
    /// </summary>
    public static void RemoveItems<T>(this ICollection<T> target, IList<T> items)
    {
        if (!target.Any() || !items.Any()) return;

        foreach (var item in items)
        {
            var found = target.FirstOrDefault(
                x => x != null && item != null && x.Equals(item));

            if (found != null)
                target.Remove(found);
        }
    }

    /// <summary>
    /// delete item from ObservableCollection
    /// </summary>
    public static bool RemoveItem<T>(this ObservableCollection<T> target, T item)
    {
        if (target is null || item is null) 
            return false;

        var index = target.IndexOf(item);

        if (index >= 0 && index < target?.Count)
        {
            target.RemoveAt(index);
            return true;
        }
        return false;
    }

    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> sourceNewItems)
    {
        if (collection == null || sourceNewItems?.Any() != true) return;

        sourceNewItems.ForEach(item =>
        {
            if (item != null)
                collection.Add(item);
        });
    }


    public static void ResetAndAddRange<T>(this ICollection<T> collection, IEnumerable<T> sourceNewItems)
    {
        if (collection is null || sourceNewItems?.Any() != true) 
            return;
        var list = sourceNewItems.ToList();
        collection.Clear();
        list.ForEach(item =>
        {
            if (item != null)
                collection.Add(item);
        });
    }
}
