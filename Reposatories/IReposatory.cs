namespace ProductGallary
{
    public interface IReposatory<T>
    {

        //Get All Items
        List<T> GetAll();

        // get by id
        T GetById(Guid id);  
        // insert
        bool Insert(T item);    
        // update Item
        bool Update(Guid Id,T item);    

        // delete Item
        bool Delete(Guid Id);    

     


    }
}
