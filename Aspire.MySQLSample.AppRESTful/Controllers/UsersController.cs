using Aspire.MySQLSample.AppRESTful.ViewModels;

using Aspire.MySQLSample.Core;

using Aspire.MySQLSample.Core.ApplicationServices;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aspire.MySQLSample.AppRESTful.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	/// <summary>
	/// Users the add asynchronous.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost]
	public async ValueTask<string> UserAddAsync(
		[FromServices] IMediator mediator,
		[FromBody] UserAddViewModel source)
		=> await mediator.Send(
			request: new UserAddRequest(
				Username: source.UserName,
				Password: source.Password),
		cancellationToken: HttpContext.RequestAborted)
		.ConfigureAwait(false);

	/// <summary>
	/// Users the get asynchronous.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async ValueTask<UserInfoViewModel?> UserGetAsync(
		[FromServices] IMediator mediator,
		string id)
	{
		var response = await mediator.Send(
			request: new UserGetRequest(Id: id),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return response is null
			? null
			: new UserInfoViewModel(
				response.Id,
				response.Username,
				response.State,
				response.CreatedAt,
				response.UpdateAt);
	}

	/// <summary>
	/// Users the get by username asynchronous.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="username">The username.</param>
	/// <returns></returns>
	[HttpGet("{username}/username")]
	public async ValueTask<UserInfoViewModel?> UserGetByUsernameAsync(
		[FromServices] IMediator mediator,
		string username = "gordon.hung")
	{
		var response = await mediator.Send(
			request: new UserGetByUsernameRequest(Username: username),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return response is null
			? null
			: new UserInfoViewModel(
				response.Id,
				response.Username,
				response.State,
				response.CreatedAt,
				response.UpdateAt);
	}
}
