using System;
using System.Threading.Tasks;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Features.Users.Abstractions;

public interface IUserCreationEngine
{
    Task<string> CreateUserAsync(NewUserDto newUser);
}