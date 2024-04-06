using DataProvider.Data;
using Microsoft.Extensions.Logging;

namespace DataProvider.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public IAccountRepository Account { get; }
        public ICategoryRepository Category { get; }
        public ITransactionRepository Transaction { get; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            var logger = loggerFactory.CreateLogger("logs");

            Account = new AccountRepository(_context, logger);
            Category = new CategoryRepository(_context, logger);
            Transaction = new TransactionRepository(_context, logger);
        }

        public async Task<bool> CompleteAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

