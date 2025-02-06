using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Queries.Assets.Requests;

public record struct GetAssetTypeByIdQuery(
    string Id
) : IRequest<AssetType?>;