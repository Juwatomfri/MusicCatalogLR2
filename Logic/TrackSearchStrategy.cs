namespace Logic
{
    public class TrackSearchStrategy : ISearchStrategy
    {
        public List<object> Search(Catalog catalog, string query)
        {
            return catalog.Singers
                .SelectMany(al => al.Tracks)
                .Where(t => t.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Cast<object>().ToList();
        }
    }
}
