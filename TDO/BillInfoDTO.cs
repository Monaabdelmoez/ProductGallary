namespace ProductGallary.TDO
{
    public class BillInfoDTO
    {
        public Guid Id { get; set; }
        public float Price { get; set; }
        public string userName { get;set; }
        public List<ProductInfoDTO> products;
        public string currentDate { get; set; }
        public string currentTime { get; set; }
    }
}
