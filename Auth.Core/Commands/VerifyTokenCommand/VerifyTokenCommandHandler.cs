using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Models;
using MediatR;

namespace Auth.Core.Commands.VerifyTokenCommand
{
    public class VerifyTokenCommandHandler : IRequestHandler<VerifyTokenCommand, VerifyTokenCommandResponse>
    {
        private readonly ITokenService _tokenService;

        public VerifyTokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<VerifyTokenCommandResponse> Handle(VerifyTokenCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!_tokenService.TryDecodeToken(request.Token, out TokenPayloadModel token))
            {
                return VerifyTokenCommandResponse.Invalid("Token is not in a valid format");
            }

            bool isValid = await _tokenService.ValidateToken(request.Token, token);
            return isValid == true ?
                VerifyTokenCommandResponse.Valid(token.ExpirationDate) :
                VerifyTokenCommandResponse.Invalid("Token is expired or revoked");
        }
    }
}
