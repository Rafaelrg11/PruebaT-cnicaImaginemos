using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaImaginemos.Application.Commands.Sale;
using PruebaTecnicaImaginemos.Application.Commands.Sale.CreateSale;
using PruebaTecnicaImaginemos.Application.Commands.Sale.DeleteSale;
using PruebaTecnicaImaginemos.Application.Commands.Sale.GetSale;
using PruebaTecnicaImaginemos.Application.Commands.Sale.GetSales;
using PruebaTecnicaImaginemos.Application.Commands.Sale.PaginationSale;
using PruebaTecnicaImaginemos.Application.Commands.Sale.UpdateSale;
using PruebaTecnicaImaginemos.Domain.DTOs.Sale;

namespace PruebaTécnicaImaginemos.ApiView.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaleController : Controller
{
    private readonly ISender _sender;

    public SaleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateSale")]
    public async Task<IActionResult> CreateSale(SaleDTO2 request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateSaleCommand(request.UserId));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetSales{id}")]
    public async Task<IActionResult> GetSale(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetSaleQuery(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetSales")]
    public async Task<IActionResult> GetAllSale()
    {
        var response = await _sender.Send(new GetSalesQuery());

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpPut("UpdateSale{id}")]
    public async Task<IActionResult> UpdateSale(SaleDTO2 sale, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateSaleCommand(sale));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("PaginationSale")]
    public async Task<IActionResult> PaginationSale(int skip = 0, int limit = 12)
    {
        var response = await _sender.Send(new PaginationSaleQuery(skip, limit));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        var dataModel = new ResponseStandar<List<SaleResponse>>()
        {
            data = response.Value.Item1,
            total = response.Value.Item2
        };

        return Ok(dataModel);
    }

    [HttpDelete("DeleteSale{id}")]
    public async Task<IActionResult> DeleteSale(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteSaleCommand(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }
}
