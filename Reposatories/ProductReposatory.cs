using Microsoft.EntityFrameworkCore;
using ProductGallary.Models;


namespace ProductGallary.Reposatories
{
    public class ProductReposatory: IReposatory<Product> , IProduct<Product> 
    {
      
        Context context;
        public ProductReposatory(Context context)
        {
            this.context = context;
        }

        public List<Product> GetAll()
        {
             return context.Products.ToList();
           
        }
        
        public Product GetById(Guid id)
        {
            return context.Products.FirstOrDefault(x=>x.Id ==id);
        }
        public bool Insert( Product item)
        {
            try
            {
                context.Products.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception e )
            {
                return false;
            }
           

        }
        public bool Update(Guid id, Product item)
        {          
                try
                 { 
                           
                    Product OldProduct = GetById(id);
                    OldProduct.Name = item.Name;
                    OldProduct.Image = item.Image;
                    OldProduct.Price = item.Price;
                    OldProduct.HasDiscount = item.HasDiscount;
                    OldProduct.DiscountPercentage = item.DiscountPercentage;
                    OldProduct.Description = item.Description;
                    OldProduct.User_Id = item.User_Id;
                    OldProduct.Category_Id = item.Category_Id;
                    OldProduct.Gallary_Id = item.Gallary_Id;
                                 
                    context.SaveChanges();
                    return true;
                
                }
                catch (Exception e)
                {
                    return false;
                }
                                 
        }
        public bool Delete(Guid id)
        {
            try
            {
                Product OldProduct = GetById(id);
                context.Products.Remove(OldProduct);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Product> GetAllProductsWithCategoryData(Guid catid )
        {
            return context.Products.Where(e=>e.Category_Id==catid).ToList();
        }
        public List<Product> GetAllProductsWithGallaryId(Guid catid )
        {
            return context.Products.Where(e=>e.Gallary_Id==catid).ToList();
        }



        public List<Product> GetAllProductsWithGallaryData(string id )
        {
            return context.Products.Where(e=>e.User_Id==id).ToList();

        }


    }
}
