using MediatR;
using Aspire.MySQLSample.Core.ApplicationServices;

namespace Aspire.MySQLSample.Core.ApplicationServices;
public record UserGetByIdRequest(
	string Id) : IRequest<UserInfoResponse?>;
