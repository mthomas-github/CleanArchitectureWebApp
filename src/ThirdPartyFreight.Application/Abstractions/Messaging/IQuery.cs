using ThirdPartyFreight.Domain.Abstractions;
using MediatR;

namespace ThirdPartyFreight.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>    
{
    
}