namespace ProductGallary.Reposatories
{
    public interface IFilter<T>
    {
        List<T> filter(string id);
    }
}
