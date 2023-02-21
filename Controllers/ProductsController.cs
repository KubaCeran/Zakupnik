using Microsoft.AspNetCore.Mvc;
using Zakupnik.Data.Entities;
using Zakupnik.Repository;
using Zakupnik.Repository.Category;
using Zakupnik.Repository.Product;

namespace Zakupnik.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddProduct(string productName, string categoryName)
        {
            var productToAdd = await _productRepository.GetProductByName(productName);
            
            if (productToAdd != null)
            {
                return BadRequest($"{productName} already exists");
            }

            var category = await _categoryRepository.FindCategoryByName(categoryName);
            var product = new Product{
                ProductName = productName,
            };

            if (category != null)
            {
                product.Category = category;
            }
            else
            {
                product.Category = new Category { CategoryName = categoryName};
            }
                        
            _productRepository.AddProduct(product);

            if(await _productRepository.SaveAllAsync())
            {
                return Ok(new ProductDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName
                });
            }

            return BadRequest("Failed to add the product");
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return BadRequest("Cannot find the product");

            }
            _productRepository.DeleteProduct(product);

            if (await _productRepository.SaveAllAsync()) 
                return Ok($"{product.ProductName} deleted successfully");
            return BadRequest("Problem deleting the post");
            
        }

        [HttpDelete("category/{id}")]
        public async Task<ActionResult> DeleteAllProductsFromCategory(int id)
        {
            var category = await _categoryRepository.FindCategoryById(id);

            if (category == null)
            {
                return BadRequest("Category doesn't exist");
            }

            await _productRepository.DeleteAllProductsForCategory(id);

            if (await _productRepository.SaveAllAsync())
                return Ok($"All products from {category.CategoryName} deleted successfully");
            return BadRequest($"Problem deleting products from {category.CategoryName}");

        }
    }
}
