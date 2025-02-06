using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public struct UpdateAssetTypeCommand: IRequest<bool>
{
    public string? Id { get; set; }
    public string Description { get; set; }
    public bool? Active { get; set; }
    public string? Icon { get; set; }

    public UpdateAssetTypeCommand(string description, bool active = true, string? icon = null)
    {
        Description = description;
        Active = active;
        Icon = icon;
    }

    public UpdateAssetTypeCommand(AssetType assetType)
    {
        Id = assetType.Id;
        Description = assetType.Description;
        Active = assetType.Active;
        Icon = assetType.Icon;
    }
}