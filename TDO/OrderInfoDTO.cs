namespace ProductGallary.TDO
{
    public class OrderInfoDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsCanceled { get; set; }
        public List<ProductInfoDTO> Products { get; set; }
    }
}
