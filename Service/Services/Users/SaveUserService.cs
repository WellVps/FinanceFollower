using Domain.Domains.Users;
using Infraestructure.Repositories.Interfaces;
using Service.Cqrs.Commands.Users.Requests;
using Service.Services.Authentication.Interfaces;
using Service.Services.Users.Interfaces;
using Service.Utils;

namespace Service.Services.Users;

public class SaveUserService(
    IUserRepository _userRepository,
    IPasswordHasher passwordHasher
) : ISaveUserService
{
    public async Task<User> SaveUser(CreateUserRequest user, CancellationToken cancellationToken = default)
    {
        // Add Email validation
        
        var hasUser = await _userRepository.HasRecord(x => x.Email == user.Email, cancellationToken);
        if (hasUser)
        {
            throw new Exception("User already exists");
        }

        var password = user.Password ?? PasswordHelper.GenerateRandomPassword();
        var hashedPassword = passwordHasher.HashPassword(password);

        var newUser = new User(user.Name, user.Email, hashedPassword, user.Role);
        var isValid = newUser.Validate();
        if (!isValid.IsValid)
        {
            throw new Exception("Invalid user");
        }

        var userWasSaved = await _userRepository.Save(newUser, cancellationToken);


        return userWasSaved ? newUser : null;
    }
}