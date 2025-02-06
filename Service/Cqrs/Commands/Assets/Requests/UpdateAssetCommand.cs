using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public struct UpdateAssetCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }
    public string IdAssetType { get; set; }
    public bool? Active { get; set; }

    public UpdateAssetCommand(string id, string ticker, string name, string idAssetType, bool? active = true)
    {
        Id = id;
        Ticker = ticker;
        Name = name;
        IdAssetType = idAssetType;
        Active = active;
    }

    public UpdateAssetCommand(Asset asset)
    {
        Id = asset.Id;
        Ticker = asset.Ticker;
        Name = asset.Name;
        IdAssetType = asset.IdAssetType;
        Active = asset.Active;
    }
}