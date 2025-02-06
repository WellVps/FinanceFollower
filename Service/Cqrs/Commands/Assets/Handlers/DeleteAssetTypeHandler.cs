using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class DeleteAssetTypeHandler : IRequestHandler<DeleteAssetTypeCommand, bool>
{
    private readonly IMediator _mediator;
    
    public DeleteAssetTypeHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var assetType = await _mediator.Send(new GetAssetTypeByIdQuery(request.Id), cancellationToken);

        if(assetType is null)
            return false; // TODO change to throw a NotFoundException

        assetType.Active = false;
        return await _mediator.Send(new UpdateAssetTypeCommand(assetType), cancellationToken);
    }
}