using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Queries.Assets.Requests;

public class GetAssetsQuery : IRequest<IEnumerable<Asset>> {}