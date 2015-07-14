namespace LorealOptimiseBusiness.Lists
{
    public delegate void EntityChangedEventHandler<T> (object sender, T entity);

    public interface IModify<T>
    {
        void InsertOrUpdate(T entity);

        void Delete(T entity);

        event EntityChangedEventHandler<T> EntityChanged;
    }
}
