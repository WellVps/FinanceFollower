using System;
using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Handlers;

public class GetInvestmentsByAssetIdHandler: IRequestHandler<GetInvestmentsByAssetIdQuery, List<Investments>>
{
    private readonly IInvestmentsRepository _investmentsRepository;

    public GetInvestmentsByAssetIdHandler(IInvestmentsRepository investmentsRepository)
    {
        _investmentsRepository = investmentsRepository;
    }

    public async Task<List<Investments>> Handle(GetInvestmentsByAssetIdQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Investments>.Filter.And(
            Builders<Investments>.Filter.Eq(x => x.AssetId, request.AssetId),
            Builders<Investments>.Filter.Gt(x => x.TotalAmount, 0)
        );

        var investments = await _investmentsRepository.GetAll(filter, cancellationToken: cancellationToken);

        if (investments.Any())
            throw new KeyNotFoundException("No investments found.");

        return [.. investments];
    }
}
