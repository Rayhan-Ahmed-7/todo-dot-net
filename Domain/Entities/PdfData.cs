namespace Domain.Entities
{
    public class PdfData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
