using BaseInfraestructure;
using Infraestructure.MongoContexts.Interfaces;
using MongoDB.Driver;

namespace Infraestructure.MongoContexts;

public class MainContext: MongoContextBase, IMainContext
{
    public MainContext(IMongoDatabase writeDatabase, IMongoDatabase readDatabase) : base(writeDatabase, readDatabase)
    {
    }
}