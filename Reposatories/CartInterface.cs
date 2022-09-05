using ProductGallary.Models;

namespace ProductGallary.Reposatories
{
    public interface CartInterface
    {
        public Guid ID { get; set; }
        bool Add(Cart item);
        bool Delete(Guid id);
        Cart GetById(Guid id);
        List<Cart> GetAll();


    }
}
