using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Repositories;
using MediatR;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IRevokedTokenRepository _revokedTokenRepository;

        public LogoutUserCommandHandler(IRevokedTokenRepository revokedTokenRepository)
        {
            _revokedTokenRepository = revokedTokenRepository ?? throw new ArgumentNullException(nameof(revokedTokenRepository));
        }

        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            await _revokedTokenRepository.InsertAsync(request.Token);

            return Unit.Value;
        }
    }
}
