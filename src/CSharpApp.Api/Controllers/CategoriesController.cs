using CSharpApp.Api.Attributes;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CSharpApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/categories")]
    [ApiController]
    [Auth]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly RestApiSettings _restApiSettings;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ILogger<CategoriesController> logger, IOptions<RestApiSettings> restApiSettings, ICategoriesService categoriesService)
        {
            _logger = logger;
            _restApiSettings = restApiSettings.Value;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Category>>> Get(CancellationToken ct)
        {
            return Ok(await _categoriesService.GetCategories(ct));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get([FromRoute] int id, CancellationToken ct)
        {
            return Ok(await _categoriesService.GetCategoryById(id, ct));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] CreateCategory category, CancellationToken ct)
        {
            return Ok(await _categoriesService.AddCategory(category, ct));
        }
    }
}
