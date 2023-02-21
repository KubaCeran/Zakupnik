using Microsoft.EntityFrameworkCore;
using Zakupnik.Data.DataContext;

namespace Zakupnik.Repository.Product
{
    public interface IProductRepository
    {
        void AddProduct(Data.Entities.Product product);
        void DeleteProduct(Data.Entities.Product product);
        Task DeleteAllProductsForCategory(int categoryId);
        Task<bool> SaveAllAsync();
        Task<List<ProductDto>> GetAllProducts();
        Task<Data.Entities.Product> GetProductByName(string productName);
        Task<Data.Entities.Product> GetProductById(int id);
        Task<List<ProductDto>> GetAllProductsForCategory(int id);

    }
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async void AddProduct(Data.Entities.Product product)
        {
            await _dataContext.Products.AddAsync(product);
        }

        public async Task DeleteAllProductsForCategory(int categoryId)
        {
            var productsDto = await GetAllProductsForCategory(categoryId);
            var products = productsDto.Select(x => new Data.Entities.Product
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId
            }).ToList();

            foreach (var product in products)
            {
                DeleteProduct(product);
            }
        }

        public void DeleteProduct(Data.Entities.Product product)
        {
            _dataContext.Products.Remove(product);
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _dataContext.Products.Select(x => new ProductDto
            { 
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                ProductId = x.ProductId,
                ProductName = x.ProductName
            }).ToListAsync();
            
            return products;

        }

        public async Task<List<ProductDto>> GetAllProductsForCategory(int id)
        {
            var products = await _dataContext.Products.Where(x => x.CategoryId == id).Select(x => new ProductDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                ProductId = x.ProductId,
                ProductName = x.ProductName
            }).ToListAsync();

            return products;

        }

        public async Task<Data.Entities.Product> GetProductById(int id)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            return product;
        }

        public async Task<Data.Entities.Product> GetProductByName(string productName)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.ProductName == productName);
            return product;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
