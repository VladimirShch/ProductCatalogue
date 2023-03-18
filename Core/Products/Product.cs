namespace TaskWPFExperiment.Core.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProductType Type { get; set; }
        public int Price { get; set; }

    }
}
