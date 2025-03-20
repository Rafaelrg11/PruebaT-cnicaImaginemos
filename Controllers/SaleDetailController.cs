using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.CreateSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.DeleteSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.GetAllSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.GetSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.PaginationSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.SaleDetail.UpdateSaleDetail;
using PruebaTecnicaImaginemos.Application.Commands.User;
using PruebaTecnicaImaginemos.Application.Commands.User.CreateUser;
using PruebaTecnicaImaginemos.Application.Commands.User.DeleteUser;
using PruebaTecnicaImaginemos.Application.Commands.User.GetUser;
using PruebaTecnicaImaginemos.Application.Commands.User.GetUsers;
using PruebaTecnicaImaginemos.Application.Commands.User.PaginationUser;
using PruebaTecnicaImaginemos.Application.Commands.User.UpdateUser;
using PruebaTecnicaImaginemos.Domain.DTOs.SaleDetail;
using PruebaTecnicaImaginemos.Domain.DTOs.User;

namespace PruebaTécnicaImaginemos.ApiView.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaleDetailController : Controller
{
    private readonly ISender _sender;

    public SaleDetailController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateSaleDetail")]
    public async Task<IActionResult> CreateSaleDetail(SaleDetailDTO2 detail, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateDetailSaleCommand(detail.IdProduct, detail.IdSale, 
            detail.Amount, detail.UnitPrice));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetSaleDetail{id}")]
    public async Task<IActionResult> GetSaleDetail(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetSaleDetailQuery(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("GetAllDetails")]
    public async Task<IActionResult> GetAllDetails()
    {
        var response = await _sender.Send(new GetAllSaleDetailQuery());

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpPut("UpdateDetails{id}")]
    public async Task<IActionResult> UpdateDetails(SaleDetailDTO2 detail, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateSaleDetailCommand(detail));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("PaginationDetails")]
    public async Task<IActionResult> PaginationDetails(int skip = 0, int limit = 12)
    {
        var response = await _sender.Send(new PaginationSaleDetailQuery(skip, limit));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        var dataModel = new ResponseStandar<List<SaleDetailResponse>>()
        {
            data = response.Value.Item1,
            total = response.Value.Item2
        };

        return Ok(dataModel);
    }

    [HttpDelete("DeleteDetails{id}")]
    public async Task<IActionResult> DeleteDetails(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteSaleDetailCommand(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }
}
