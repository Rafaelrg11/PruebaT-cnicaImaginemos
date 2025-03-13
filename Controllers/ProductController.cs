using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaImaginemos.Application.Commands.Product;
using PruebaTecnicaImaginemos.Application.Commands.Product.CreateProduct;
using PruebaTecnicaImaginemos.Application.Commands.Product.DeleteProduct;
using PruebaTecnicaImaginemos.Application.Commands.Product.GetAllProduct;
using PruebaTecnicaImaginemos.Application.Commands.Product.GetProduct;
using PruebaTecnicaImaginemos.Application.Commands.Product.Pagination;
using PruebaTecnicaImaginemos.Application.Commands.Product.UpgradeProduct;
using PruebaTecnicaImaginemos.Domain.DTOs.Product;

namespace PruebaTécnicaImaginemos.ApiView.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct(ProductDTO dTO, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateProductCommand(dTO.NameProduct, dTO.Price, dTO.Description));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetProduct{id}")]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetProductQuery(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var response = await _sender.Send(new GetProductsQuery());

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpPut("UpdateProduct{id}")]
    public async Task<IActionResult> UpdateProduct(ProductDTO2 userDTO, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateProductsCommand(userDTO));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("PaginationProduct")]
    public async Task<IActionResult> PaginationProduct(int skip = 0, int limit = 12)
    {
        var response = await _sender.Send(new ProductPaginationQuery(skip, limit));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        var dataModel = new ResponseStandar<List<ProductsResponse>>()
        {
            data = response.Value.Item1,
            total = response.Value.Item2
        };

        return Ok(dataModel);
    }

    [HttpDelete("DeleteProduct{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteProductCommand(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }
}
