using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Queries.Assets.Requests;

public class GetAssetTypesRequest : IRequest<IEnumerable<AssetType>> {}