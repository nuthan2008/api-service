using System;
using DataProvider.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataProvider.Repositories
{
	public class AccountRepository:GenericRepository<Account>, IAccountRepository
	{
		public AccountRepository(AppDbContext context, ILogger logger): base(context, logger)
		{
		}

		public override async Task<IEnumerable<Account>> All()
		{
			try
			{
				return await _dbSet.Where(x => x.Status == 1)
					.AsNoTracking()
					.AsSplitQuery()
					.OrderBy(x => x.CreatedDate)
					.ToListAsync();

			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "{Repo} All function error", typeof(AccountRepository));
				throw;
			}
		}

        public override async Task<bool> Update(Account account)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == account.Id);

                if (result == null)
                    return false;

				result.ModifiedDate = DateTime.UtcNow;
				result.Name = account.Name;
				result.Description = account.Description;
				result.isActive = account.isActive;

				return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(AccountRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(AccountRepository));
                throw;
            }
        }
	}
}

