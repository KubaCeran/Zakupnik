using Microsoft.AspNetCore.Mvc;
using Zakupnik.Repository;

namespace Zakupnik.Controllers
{
    public class ProductsController : BaseApiController
    {

        [HttpPost("add")]
        public Task<ActionResult> AddProduct(ProductDto productDto)
        {
            var category = 
        }
    }
}
