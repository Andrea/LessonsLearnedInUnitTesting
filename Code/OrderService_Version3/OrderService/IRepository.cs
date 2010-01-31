namespace CodeSamplesUnitTestingTalk.OrderService.Version3
{
    public interface IRepository<T>
    {
        void Save(T entity);
    }
}