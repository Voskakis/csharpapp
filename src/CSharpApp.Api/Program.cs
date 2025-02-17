using CSharpApp.Core.Dtos;
using CSharpApp.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var versionedEndpointRouteBuilder = app.NewVersionedApi();

#region Auth
versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/auth/login", async (IAuthService authService) =>
{
    var token = await authService.GetAccessToken();
    return string.IsNullOrEmpty(token) ? Results.Unauthorized() : Results.Ok(new { access_token = token });
})
.WithName("Login")
.HasApiVersion(1.0);
#endregion

#region products

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
    {
        var products = await productsService.GetProducts();
        return products;
    })
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts/{id:int}", async (int id, IProductsService productsService) =>
{
    var product = await productsService.GetProductById(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
})
    .WithName("GetProductById")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/addproduct", async (CreateProduct product, IProductsService productsService) =>
{
    var createdProduct = await productsService.AddProduct(product);
    return Results.Created("api/v1.0/addproduct", createdProduct);
})
    .WithName("AddProduct")
    .HasApiVersion(1.0);
#endregion

#region categories

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetCategories();
    return categories;
})
    .WithName("GetCategories")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories/{id:int}", async (int id, ICategoriesService categoriesService) =>
{
    var category = await categoriesService.GetCategoryById(id);
    return category != null ? Results.Ok(category) : Results.NotFound();
})
    .WithName("GetCategoryById")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/addcategory", async (CreateCategory category, ICategoriesService categoriesService) =>
{
    var createdCategory = await categoriesService.AddCategory(category);
    return Results.Created("api/v1.0/addcategory", createdCategory);
})
    .WithName("AddCategory")
    .HasApiVersion(1.0);
#endregion

app.UseMiddleware<RequestPerformace>();

app.Run();