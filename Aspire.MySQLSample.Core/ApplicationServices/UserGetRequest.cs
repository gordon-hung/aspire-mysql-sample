using MediatR;
using Aspire.MySQLSample.Core.ApplicationServices;

namespace Aspire.MySQLSample.Core.ApplicationServices;
public record UserGetRequest(
	string Id) : IRequest<UserInfoResponse?>;
