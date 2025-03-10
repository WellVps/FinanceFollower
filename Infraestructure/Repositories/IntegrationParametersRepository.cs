using BaseInfraestructure;
using Domain.Domains.Parameters;
using Infraestructure.Repositories.Interfaces;
using Infraestructure.MongoContexts.Interfaces;

namespace Infraestructure.Repositories;

public class IntegrationParametersRepository(IMainContext context) : BaseRepository<IntegrationParameters>(context), IIntegrationParametersRepository
{
}