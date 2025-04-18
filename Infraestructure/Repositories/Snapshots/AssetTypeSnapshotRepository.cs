using BaseInfraestructure;
using Domain.Domains.Snapshots;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Snapshots.Interfaces;

namespace Infraestructure.Repositories.Snapshots;

public class AssetTypeSnapshotRepository(IMainContext context) : BaseRepository<AssetTypeSnapshot>(context), IAssetTypeSnapshotRepository {}