using FirstAPIProject.Authorization;
using FirstAPIProject.Data;
using FirstAPIProject.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAPIProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [LogSensitiveAction]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext dbContext,ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        [Route("")]
        [Authorize(Policy= "AgeGreaterThan25")]
        public ActionResult<IEnumerable<Product>> Get(){
            var userName = User.Identity.Name;
            var userId=((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value; 
           
            var records = _dbContext.Set<Product>().ToList();
            return Ok(records);

        }
        [HttpGet]
        [Route("{key}")]
        
        public ActionResult<IEnumerable<Product>> GetById([FromRoute(Name="key")]int id)
        {
            _logger.LogDebug("Getting product #"+ id);
            var record=_dbContext.Set<Product>().Find(id);
            if (record == null)
                _logger.LogWarning("Product #{id} was not found ", id);
            return record==null ? NotFound():Ok(record);

        }
    

        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct(Product product)
        {
            product.Id = 0;
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return Ok(product.Id);
        }
        [HttpPut]
        [Route("")]
        public ActionResult UpdateProduct(Product product)
        {
            var existingProduct = _dbContext.Set<Product>().Find(product.Id);
            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;
            _dbContext.Set<Product>().Update(existingProduct);
            _dbContext.SaveChanges();
            return Ok();

        }
        [HttpDelete]
        [Route("{id}")]

        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _dbContext.Set<Product>().Find(id);
            _dbContext.Set<Product>().Remove(existingProduct);
            _dbContext.SaveChanges();
            return Ok();

        }
    }
}
