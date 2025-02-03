using BaseInfraestructure;
using Domain.Domains.Users;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Interfaces;

namespace Infraestructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMainContext context) : base(context)
    {
    }
}