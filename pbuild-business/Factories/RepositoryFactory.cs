using Microsoft.Extensions.DependencyInjection;
using System;

namespace pbuild_business.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
