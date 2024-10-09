using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Aspire.MySQLSample.Core.ApplicationServices;

namespace Aspire.MySQLSample.Core.ApplicationServices;
public record UserGetByUsernameRequest(
	string Username) : IRequest<UserInfoResponse?>;
