//namespace Logic
//{
//    public class SingerSearchStrategy : ISearchStrategy
//    {
//        public List<object> Search(Catalog catalog, string query)
//        {
//            return catalog.Singers
//                .Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
//                .Cast<object>().ToList();
//        }
//    }
//}
