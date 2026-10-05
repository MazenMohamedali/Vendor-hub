using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Application.Common.Models;

namespace VendorHub.Application.Common.Messaging
{
    internal interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
