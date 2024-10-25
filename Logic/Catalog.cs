using Entities;

namespace Logic
{
    // реализация самого каталога
    public class Catalog
    {
        public List<Singer> Singers { get; set; } = [];

        public List<object> Search(string query, ISearchStrategy searchStrategy)
        {
            return searchStrategy.Search(this, query);
        }
    }
}
