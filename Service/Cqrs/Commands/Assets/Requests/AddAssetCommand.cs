using Domain.Domains.Assets;
using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public record struct AddAssetCommand (
    string Ticker,
    string Name,
    string IdAssetType
) :IRequest<Asset>;