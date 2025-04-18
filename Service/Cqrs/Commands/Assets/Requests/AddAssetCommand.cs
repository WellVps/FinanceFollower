using Domain.Domains.Assets;
using Domain.Enums;
using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public record struct AddAssetCommand (
    string Ticker,
    string Name,
    string IdAssetType,
    Integrator Integrator,
    string? InternalTicker = null
) :IRequest<Asset>;