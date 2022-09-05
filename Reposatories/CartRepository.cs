using Microsoft.EntityFrameworkCore;
using ProductGallary.Models;

namespace ProductGallary.Reposatories
{
    public class CartRepository:CartInterface,IFilter<Cart>
    {
        Context context;
        private Guid Cartid;
        public Guid ID { get { return Cartid; } set { Cartid = value; } }
        public CartRepository(Context context)
        {
            this.context = context;
            ID = Guid.NewGuid();
           
        }

       

        public bool Add(Cart item)
        {
            try
            {
                context.Carts.Add(item);
                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                var item = GetById(id);
                context.Carts.Remove(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Cart> filter(string id)
        {
            return context.Carts.Include(c=>c.products).Where(e => e.User_Id == id).ToList();
        }

        public List<Cart> GetAll()
        {
            return context.Carts.Include(c=>c.products).ToList();
        }

        public Cart GetById(Guid id)
        {
            return context.Carts.Include(cr=>cr.products).FirstOrDefault(x => x.Id == id);
            

        }
    }
}

