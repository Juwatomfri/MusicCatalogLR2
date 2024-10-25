namespace Logic
{
    //реализация поиска
    public interface ISearchStrategy
    {
        List<object> Search(Catalog catalog, string query);
    }
}
