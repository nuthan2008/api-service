using System;
using DataProvider.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataProvider.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Category>> All()
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
                _logger.LogError(ex, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }

        public override async Task<bool> Update(Category category)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == category.Id);

                if (result == null)
                    return false;

                result.ModifiedDate = DateTime.UtcNow;
                result.Name = category.Name;
                result.CategoryId = category.CategoryId;

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(CategoryRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(CategoryRepository));
                throw;
            }
        }
    }
}

