namespace OsrsTool.Domain.Interfaces
{
    public interface IOsrsApiService
    {
        Task FetchAndStoreItemsAsync();
        Task FetchAndStoreLatestPricesAsync();
    }
}
