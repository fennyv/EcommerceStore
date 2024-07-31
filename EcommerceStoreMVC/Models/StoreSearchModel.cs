namespace EcommerceStoreMVC.Models
{
    public class StoreSearchModel
    {
        public string? Search { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        public string SortOrder { get; set; } = string.Empty;

    }
}
