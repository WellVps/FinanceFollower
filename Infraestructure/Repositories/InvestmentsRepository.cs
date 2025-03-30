using BaseInfraestructure;
using Domain.Domains.Investments;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Interfaces;

namespace Infraestructure.Repositories;

public class InvestmentsRepository(IMainContext context) : BaseRepository<Investments>(context), IInvestmentsRepository {}