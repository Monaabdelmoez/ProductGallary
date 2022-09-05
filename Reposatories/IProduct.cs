namespace ProductGallary.Reposatories
{
    public interface IProduct<T>
    {

        List<T> GetAllProductsWithCategoryData(Guid id);
        List<T> GetAllProductsWithGallaryId(Guid id);
        List<T> GetAllProductsWithGallaryData(string id );


    }
}
