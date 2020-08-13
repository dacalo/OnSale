namespace OnSale.Web.Models
{
    public class Pagination
    {
        private int _recordsPage = 10;
        private readonly int maxRecordsPage = 50;
        public int Page { get; set; } = 1;
        public int RecordsPage
        { 
            get => _recordsPage;
            set => _recordsPage = (value > maxRecordsPage) ? maxRecordsPage : value;
        }
    }
}
