using ProductGallary.Models;

namespace ProductGallary.Reposatories
{
    public class CategoryRepository : IReposatory<Category>
    {
        Context context;
        public CategoryRepository(Context context)
        {
            this.context = context;
        }
        public bool Delete(Guid Id)
        {
            try {
                Category oldcat = GetById(Id);
                context.Categories.Remove(oldcat);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(Guid id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }

        public bool Insert(Category item)
        {
            try
            {
               
                
                context.Categories.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
            
        }

        public bool Update(Guid Id, Category item)
        {
            try {
                Category oldcat = GetById(Id);

                oldcat.Name = item.Name;
                oldcat.User_Id = item.User_Id;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
           
        }
    }
}
