namespace DataProvider.Repositories
{
    public interface IUnitOfWork
	{
		IAccountRepository Account { get; }

        ICategoryRepository Category { get; }

		ITransactionRepository Transaction { get; }

        Task<bool> CompleteAsync();
    }
}