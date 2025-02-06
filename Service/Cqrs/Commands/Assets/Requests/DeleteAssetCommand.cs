using MediatR;
namespace Service.Cqrs.Commands.Assets.Requests;

public record struct DeleteAssetCommand(string Id) : IRequest<bool>;