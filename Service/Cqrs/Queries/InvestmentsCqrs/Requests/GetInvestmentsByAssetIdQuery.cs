using Domain.Domains.Investments;
using MediatR;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Requests;

public record struct GetInvestmentsByAssetIdQuery(
    string AssetId
) : IRequest<List<Investments>>;
