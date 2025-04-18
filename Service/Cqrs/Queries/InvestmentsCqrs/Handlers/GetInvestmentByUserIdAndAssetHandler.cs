using MediatR;
using MongoDB.Driver;
using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Handlers;

public class GetInvestmentByUserIdAndAssetHandler(IInvestmentsRepository investmentsRepository) : IRequestHandler<GetInvestmentByUserIdAndAssetQuery, Investments>
{
    private readonly IInvestmentsRepository _investmentsRepository = investmentsRepository;

    public async Task<Investments> Handle(GetInvestmentByUserIdAndAssetQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Investments>.Filter.And(
            Builders<Investments>.Filter.Eq(x => x.UserId, request.UserId),
            Builders<Investments>.Filter.Eq(x => x.AssetId, request.AssetId)
        );

        var investment = await _investmentsRepository.GetFirstOrDefault(filter, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException("No investments found.");
        return investment;
    }
}
