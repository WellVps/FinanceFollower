using BaseInfraestructure;
using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using Infraestructure.MongoContexts.Interfaces;

namespace Infraestructure.Repositories;

public class InvestmentEntriesRepository : BaseRepository<InvestmentEntry>, IInvestmentEntriesRepository 
{
    public InvestmentEntriesRepository(IMainContext context) : base(context) {}
}