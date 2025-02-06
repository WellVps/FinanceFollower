using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class DeleteAssetHandler : IRequestHandler<DeleteAssetCommand, bool>
{
    private readonly IMediator _mediator;
    
    public DeleteAssetHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _mediator.Send(new GetAssetByIdQuery(request.Id), cancellationToken);

        if(asset is null)
            return false; // TODO change to throw a NotFoundException

        asset.Active = false;
        return await _mediator.Send(new UpdateAssetCommand(asset), cancellationToken);
    }
}