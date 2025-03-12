using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTécnicaImaginemos.ApiView;
using PruebaTecnicaImaginemos.Application.Commands.User;
using PruebaTecnicaImaginemos.Application.Commands.User.CreateUser;
using PruebaTecnicaImaginemos.Application.Commands.User.DeleteUser;
using PruebaTecnicaImaginemos.Application.Commands.User.GetUser;
using PruebaTecnicaImaginemos.Application.Commands.User.GetUsers;
using PruebaTecnicaImaginemos.Application.Commands.User.PaginationUser;
using PruebaTecnicaImaginemos.Application.Commands.User.UpdateUser;
using PruebaTecnicaImaginemos.Domain.DTOs.User;

namespace PruebaTécnicaImaginemos.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(UserDTO userDTO, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateUserCommand(userDTO.Name, userDTO.DNI));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response);
    }

    [HttpGet("[GetUser]")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetUsersQuery(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("[GetAllUsers]")]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _sender.Send(new GetAllUsersQuery());

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpPut("[UpdateUser]")]
    public async Task<IActionResult> UpdateUser(UserDTO2 userDTO, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateUserCommand(userDTO));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }

    [HttpGet("[PaginationUser]")]
    public async Task<IActionResult> PaginationUser(int skip = 0, int limit = 12)
    {
        var response = await _sender.Send(new PaginationUserQuery(skip, limit));

        if (!response.IsSuccess)
        {
            return BadRequest(response.IsFailure);
        }

        var dataModel = new ResponseStandar<List<UserResponse>>()
        {
            data = response.Value.Item1,
            total = response.Value.Item2
        };

        return Ok(dataModel);
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteUserCommand(id));

        if (!response.IsSuccess) 
        {
            return BadRequest(response.IsFailure);
        }

        return Ok(response.Value);
    }
}
