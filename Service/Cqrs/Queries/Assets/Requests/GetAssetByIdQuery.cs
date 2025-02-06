using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Queries.Assets.Requests;

public record struct GetAssetByIdQuery(string Id) : IRequest<Asset?>;