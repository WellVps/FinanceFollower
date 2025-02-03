using BaseInfraestructure;
using Domain.Domains.Users;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Interfaces;

namespace Infraestructure.Repositories;

public class UserRefreshTokenRepository : BaseRepository<UserRefreshToken>, IUserRefreshTokenRepository
{
    public UserRefreshTokenRepository(IMainContext context) : base(context)
    {
    }
}