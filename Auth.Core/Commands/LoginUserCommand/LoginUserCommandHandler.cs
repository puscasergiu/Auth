using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Exceptions;
using Auth.Core.Repositories;
using FluentValidation;
using MediatR;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly HashCrypter _crypter;
        private readonly ITokenService _tokenService;
        private readonly AbstractValidator<LoginUserCommand> _validator;

        public LoginUserCommandHandler(IUserRepository userRepository, HashCrypter hashCrypter, ITokenService tokenService, AbstractValidator<LoginUserCommand> validator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _crypter = hashCrypter ?? throw new ArgumentNullException(nameof(hashCrypter));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var user = await _userRepository.TryGetAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (user == null || !_crypter.Verify(request.Password, user.HashedPassword, user.Salt))
            {
                throw new DomainException("Wrong username or password");
            }

            var token = _tokenService.CreateToken(user.Id, user.Username);

            return new LoginUserCommandResponse()
            {
                Token = token
            };
        }
    }
}
