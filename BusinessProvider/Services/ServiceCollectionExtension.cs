using System;
using BusinessProvider.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessProvider.Services
{
    public enum Types
    {
       Practices,
       SoloPractices
    }

    public static class ServiceCollectionExtension
	{
		public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection serviceCollection)
			where TInterface: class
			where TImplementation: class, TInterface
		{
			serviceCollection.AddTransient<TInterface, TImplementation>();
			serviceCollection.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
			serviceCollection.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
		}
	}

	public class AbstractFactory<T>: IAbstractFactory<T>
	{
		private readonly Func<T> _factory;
		public AbstractFactory(Func<T> factory)
		{
			_factory = factory;
        }

		public T Create()
		{
			return _factory();
		}
    }

	public interface IAbstractFactory<T>
	{
		T Create();
    }
}