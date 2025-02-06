using BaseInfraestructure;
using Domain.Domains.Assets;
using Infraestructure.MongoContexts.Interfaces;
using Infraestructure.Repositories.Interfaces;

namespace Infraestructure.Repositories;

public class AssetTypeRepository : BaseRepository<AssetType>, IAssetTypeRepository
{
    public AssetTypeRepository(IMainContext context) : base(context) {}
}