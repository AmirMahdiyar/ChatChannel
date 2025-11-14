using Microsoft.Extensions.DependencyInjection;

namespace AliLotfiStrategy
{
    public static class PokhtoPazFile
    {
        public static IServiceCollection AddScopedLazy<TContract, TImplementation, TKey>(this IServiceCollection serviceCollection, TKey key)
            where TImplementation : class, TContract
            where TContract : class
        {
            serviceCollection.AddScoped<TImplementation>();


            serviceCollection.AddScoped(serviceProvider => new LazyServiceRetriever<TContract, TKey>(key, new Lazy<TContract>(() => serviceProvider.GetRequiredService<TImplementation>())));

            return serviceCollection;
        }
    }

    public class LazyServiceRetriever<TService, TKey>
    {
        public LazyServiceRetriever(TKey key, Lazy<TService> lazyService)
        {
            Key = key;
            LazyService = lazyService;
        }

        public TKey Key { get; private set; }
        public Lazy<TService> LazyService { get; private set; }
    }

    public interface IStrategy<TStrategy, TKey>
    {
        public TStrategy GetStrategy(TKey key);
    }
    public class StrategyRepository<TRepository, TKey> : IStrategy<TRepository, TKey>
    {
        private readonly IEnumerable<LazyServiceRetriever<TRepository, TKey>> _repository;

        public StrategyRepository(IEnumerable<LazyServiceRetriever<TRepository, TKey>> repository)
        {
            _repository = repository;
        }

        public TRepository GetStrategy(TKey key)
        {
            return _repository
                   .Single(x => EqualityComparer<TKey>.Default.Equals(x.Key, key))
                   .LazyService.Value;
        }
    }

}
