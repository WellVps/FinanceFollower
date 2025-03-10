using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Queries.Assets.Requests;

public record struct GetAssetByTickerQuery(string Ticker) : IRequest<Asset>;