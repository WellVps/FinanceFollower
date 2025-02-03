using BaseInfraestructure;
using Domain.Domains.Assets;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Interfaces;

namespace Infraestructure.Repositories;

public class AssetRepository : BaseRepository<AssetType>, IAssetRepository
{
    public AssetRepository(IMainContext context) : base(context) {}
}