using ThirdPartyFreight.Domain.Abstractions;
using MediatR;

namespace ThirdPartyFreight.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> 
    where TQuery : IQuery<TResponse>
{
    
}