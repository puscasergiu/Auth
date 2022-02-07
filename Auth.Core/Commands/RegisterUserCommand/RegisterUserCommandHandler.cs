using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Cryptography;
using Auth.Core.Exceptions;
using Auth.Core.Models;
using Auth.Core.Repositories;
using FluentValidation;
using MediatR;

namespace Auth.Core.Commands.RegisterUserCommand
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashCrypter _crypter;
        private readonly AbstractValidator<RegisterUserCommand> _validator;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, HashCrypter hashCrypter, AbstractValidator<RegisterUserCommand> validator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _crypter = hashCrypter ?? throw new ArgumentNullException(nameof(hashCrypter));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var usernameExists = await _userRepository.ExistsAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (usernameExists)
            {
                throw new DomainException($"Username {request.Username} is already taken.");
            }

            var (hashedPassword, passwordSalt) = _crypter.Hash(request.Password);
            var user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                HashedPassword = hashedPassword,
                Salt = passwordSalt
            };

            _userRepository.Insert(user);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
