using MediatR;

namespace Aspire.MySQLSample.Core.ApplicationServices;
public record UserAddRequest(
	string Username,
	string Password) : IRequest<string>;
