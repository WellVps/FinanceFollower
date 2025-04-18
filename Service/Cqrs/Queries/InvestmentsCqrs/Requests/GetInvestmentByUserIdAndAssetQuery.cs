using MediatR;
using Domain.Domains.Investments;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Requests;

public record struct GetInvestmentByUserIdAndAssetQuery (
    string UserId,
    string AssetId
): IRequest<Investments>;