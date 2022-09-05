namespace ProductGallary.TDO
{
    public class ProductInfoDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public float? Price { get; set; }
        public float? DiscountPercentage { get; set; }
    }
}
