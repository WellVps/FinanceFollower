using MediatR;
using Domain.Domains.Assets;

namespace Service.Cqrs.Commands.Snapshots.Requests;

public record struct CreateAssetSnapshotCommand(
    Asset Asset
) : IRequest;