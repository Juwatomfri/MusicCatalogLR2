using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logic
{

    public class AlbumSearchStrategy : ISearchStrategy
    {
        public List<object> Search(Catalog catalog, string query)
        {
            return catalog.Singers
                .SelectMany(a => a.Albums)
                .Where(al => al.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Cast<object>().ToList();
        }
    }
}
