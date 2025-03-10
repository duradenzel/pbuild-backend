namespace pbuild_business.Factories
{
    public interface IRepositoryFactory
    {
        T CreateRepository<T>() where T : class;
    }
}
