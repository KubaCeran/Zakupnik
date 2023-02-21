using Microsoft.AspNetCore.Mvc;
using Zakupnik.Data.DataContext;
using Zakupnik.Data.Entities;
using Zakupnik.Repository;
using Zakupnik.Repository.Category;
using Zakupnik.Repository.Product;

namespace Zakupnik.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        [HttpGet()]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            return await _categoryRepository.GetAll();
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCategory(string categoryName)
        {
            var categoryToAdd = await _categoryRepository.FindCategoryByName(categoryName);

            if (categoryToAdd != null)
            {
                return BadRequest($"{categoryName} already exists");
            }

            var category = new Category
            {
                CategoryName = categoryName
            };

            _categoryRepository.AddCategory(category);

            if (await _categoryRepository.SaveAllAsync())
            {
                return Ok(new CategoryDto
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                });
            }

            return BadRequest("Failed to add the product");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductDto>>> GetAllProductsForCategory(int id)
        {
            var category = await _categoryRepository.FindCategoryById(id);

            if (category == null)
            {
                return BadRequest("Category doesn't exist");
            }

            var products = await _productRepository.GetAllProductsForCategory(id);

            if (products.Count() == 0)
            {
                return Ok($"No products found in category {category.CategoryName}");
            }
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryWithProducts(int id)
        {
            var category = await _categoryRepository.FindCategoryById(id);
            if (category == null)
            {
                return BadRequest("Category doesn't exist");
            }
            await _productRepository.DeleteAllProductsForCategory(id);
            _categoryRepository.DeleteCategory(category);
            if (await _categoryRepository.SaveAllAsync())
                return Ok($"{category.CategoryName} with all products deleted successfully");
            return BadRequest($"Problem deleting {category.CategoryName}");
        }
    }
}
