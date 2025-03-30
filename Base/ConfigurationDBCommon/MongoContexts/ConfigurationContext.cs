using MongoDB.Driver;
using BaseInfraestructure;
using ConfigurationDBCommon.MongoContexts.Interfaces;

namespace ConfigurationDBCommon.MongoContexts;

public class ConfigurationContext: MongoContextBase, IConfigurationContext
{
    public ConfigurationContext(IMongoDatabase databaseWrite, IMongoDatabase databaseRead) : base(databaseWrite, databaseRead)
    {

    }
}
