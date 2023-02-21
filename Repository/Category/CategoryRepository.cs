using Microsoft.EntityFrameworkCore;
using Zakupnik.Data.DataContext;

namespace Zakupnik.Repository.Category
{
    public interface ICategoryRepository
    {
        Task<Data.Entities.Category> FindCategoryByName(string categoryName);
        Task<Data.Entities.Category> FindCategoryById(int id);
        Task<List<CategoryDto>> GetAll();
        void AddCategory(Data.Entities.Category category);
        void DeleteCategory(Data.Entities.Category category);
        Task<bool> SaveAllAsync();
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async void AddCategory(Data.Entities.Category category)
        {
            await _dataContext.Categories.AddAsync(category);
        }

        public void DeleteCategory(Data.Entities.Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public async Task<Data.Entities.Category> FindCategoryById(int id)
        {
            return await _dataContext.Categories.SingleOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<Data.Entities.Category> FindCategoryByName(string categoryName)
        {
            return await _dataContext.Categories.SingleOrDefaultAsync(x => x.CategoryName.ToLower() == categoryName.ToLower());
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            return await _dataContext.Categories.Select(x => new CategoryDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            }).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

    }
}
