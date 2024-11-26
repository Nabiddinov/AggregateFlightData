using SharedModel.Models;

namespace BizLogicLayer.Extensions
{
    public static class SortingExtensions
    {
        public static IEnumerable<T> OrderByColumn<T>(this IEnumerable<T> source, string columnName, bool descending)
        {
            var property = typeof(T).GetProperty(columnName);
            if (property == null)
                throw new ArgumentException($"No property '{columnName}' found on type '{typeof(T).Name}'.");

            return descending
                ? source.OrderByDescending(x => property.GetValue(x, null))
                : source.OrderBy(x => property.GetValue(x, null));
        }
    }
}
