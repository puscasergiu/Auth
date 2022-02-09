using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Exceptions;
using Auth.Core.Repositories;
using MediatR;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly HashCrypter _crypter;


        public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, HashCrypter hashCrypter)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _crypter = hashCrypter ?? throw new ArgumentNullException(nameof(hashCrypter));
        }

        /// <exception cref="DomainException"></exception>"
        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var (result, user) = await _userRepository.TryGetAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (!result || !_crypter.Verify(request.Password, user.HashedPassword, Convert.FromBase64String(user.Salt)))
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
