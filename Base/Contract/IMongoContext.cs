using MongoDB.Driver;

namespace BaseContract;

public interface IMongoContext
{
    IMongoCollection<T> GetCollectionWrite<T>();
    IMongoCollection<T> GetCollectionRead<T>();
}