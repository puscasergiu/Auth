using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Exceptions;
using Auth.Core.Models;
using Auth.Core.Repositories;
using MediatR;

namespace Auth.Core.Commands.RegisterUserCommand
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashCrypter _crypter;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, HashCrypter hashCrypter)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _crypter = hashCrypter ?? throw new ArgumentNullException(nameof(hashCrypter));
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var usernameExists = await _userRepository.ExistsAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (usernameExists)
            {
                throw new DomainException($"Username {request.Username} is already taken.");
            }

            var salt = _crypter.GenerateSalt();
            var hashedPassword = _crypter.Hash(request.Password, salt);
            var user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                HashedPassword = hashedPassword,
                Salt = Convert.ToBase64String(salt)
            };

            _userRepository.Insert(user);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
