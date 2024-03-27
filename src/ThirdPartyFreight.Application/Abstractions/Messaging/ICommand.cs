using ThirdPartyFreight.Domain.Abstractions;
using MediatR;

namespace ThirdPartyFreight.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
    
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{

}

public interface IBaseCommand
{

}