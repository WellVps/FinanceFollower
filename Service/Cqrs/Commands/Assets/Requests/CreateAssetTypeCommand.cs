using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public class CreateAssetTypeCommand : IRequest<AssetType>
{
    public string Description { get; set; }
    public string? Icon { get; set; }
}
