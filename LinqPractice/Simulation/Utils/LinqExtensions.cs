using Simulation.Models;

namespace Simulation.Utils;

public static class LinqExtensions
{
    public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int page = 1, int pageSize = 1) 
    {
        page = Math.Max(page, 1);
        pageSize = Math.Max(pageSize, 1);

        return source
            .Skip((page - 1) * pageSize)
            .Take(pageSize); 
    }

    public static IEnumerable<T> GetPageActive<T>(this IEnumerable<T> source, int page, int pageSize)  where T : IActive
    {
        return source
            .Where(x => x.Active)
            .GetPage(page, pageSize);
    }
}
