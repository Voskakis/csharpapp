using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CSharpApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly RestApiSettings _restApiSettings;
        private readonly IProductsService _productsService;

        public ProductsController(ILogger<ProductsController> logger, IOptions<RestApiSettings> restApiSettings, IProductsService productsService)
        {
            _logger = logger;
            _restApiSettings = restApiSettings.Value;
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Product>>> Get(CancellationToken ct)
        {
            return Ok(await _productsService.GetProducts(ct));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get([FromRoute] int id, CancellationToken ct)
        {
            return Ok(await _productsService.GetProductById(id, ct));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] CreateProduct product, CancellationToken ct)
        {
            return Ok(await _productsService.AddProduct(product, ct));
        }
    }
}
