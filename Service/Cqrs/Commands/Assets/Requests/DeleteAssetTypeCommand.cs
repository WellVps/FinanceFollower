using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public record struct DeleteAssetTypeCommand(
    string Id
) : IRequest<bool>;