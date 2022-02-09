using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Models;
using Auth.Core.Repositories;
using MediatR;

namespace Auth.Core.Commands.LogoutUserCommand
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IRevokedTokenRepository _revokedTokenRepository;
        private readonly ITokenService _tokenService;

        public LogoutUserCommandHandler(IRevokedTokenRepository revokedTokenRepository, ITokenService tokenService)
        {
            _revokedTokenRepository = revokedTokenRepository ?? throw new ArgumentNullException(nameof(revokedTokenRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_tokenService.TryDecodeToken(request.Token, out TokenPayloadModel result) && await _tokenService.ValidateToken(request.Token, result))
            {
                await _revokedTokenRepository.InsertAsync(request.Token);
            }

            return Unit.Value;
        }
    }
}
