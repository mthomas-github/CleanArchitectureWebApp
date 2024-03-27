using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Users;
using ThirdPartyFreight.Application.Abstractions.Authentication;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = User.Create(
                new FirstName(request.FirstName),
                new LastName(request.LastName),
                new Email(request.Email));

            string identityId = await _authenticationService.RegisterAsync(
                user,
                request.Password,
                cancellationToken);

            user.SetIdentityId(identityId);

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(UserErrors.UnableToCreate);
        }
    }
}
