using System;
using DataProvider.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataProvider.Repositories
{
	public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Transaction>> All()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderBy(x => x.CreatedDate)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(TransactionRepository));
                throw;
            }
        }

        public override async Task<bool> Update(Transaction transaction)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == transaction.Id);

                if (result == null)
                    return false;

                result.ModifiedDate = DateTime.UtcNow;
                result.Title = transaction.Title;
                result.AccountId = transaction.AccountId;
                result.CategoryId = transaction.CategoryId;
                result.Amount = transaction.Amount;
                result.Description = transaction.Description;
                result.Notes = transaction.Notes;

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(TransactionRepository));
                throw;
            }
        }

        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (result == null)
                    return false;

                result.Status = 0;
                result.ModifiedDate = DateTime.UtcNow;

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(TransactionRepository));
                throw;
            }
        }
    }
}

