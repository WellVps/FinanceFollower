using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BaseInfraestructure;

public abstract class MongoContextBase
{
    protected readonly IMongoDatabase DatabaseWrite;
    protected readonly IMongoDatabase DatabaseRead;

    private static readonly object Lock = new();
    private static readonly Dictionary<Type, IBsonSerializer> CustomSerializers = new();

    protected MongoContextBase(IMongoDatabase databaseWrite, IMongoDatabase databaseRead)
    {
        DatabaseWrite = databaseWrite;
        DatabaseRead = databaseRead;

        RegisterCustomSerializers();
    }

    private static void RegisterCustomSerializers()
    {
        RegisterCustomSerializers<decimal>(BsonType.Decimal128);
    }

    private static void RegisterCustomSerializers<T>(BsonType bsonType)
    {
        var originType = typeof(T);
        var nullableOriginType = typeof(Nullable<>).MakeGenericType(originType);

        lock (Lock)
        {
            if(CustomSerializers.ContainsKey(originType)) return;

            var serializer = new DecimalSerializer(bsonType);
            var nullableSerializer = new NullableSerializer<decimal>(new DecimalSerializer(bsonType));

            BsonSerializer.RegisterSerializer(originType, serializer);
            BsonSerializer.RegisterSerializer(nullableOriginType, nullableSerializer);

            CustomSerializers.Add(key: originType, value: serializer);
        }
    }

    public IMongoCollection<T> GetCollectionWrite<T>()
    {
        return DatabaseWrite.GetCollection<T>(typeof(T).Name);
    }

    public IMongoCollection<T> GetCollectionRead<T>()
    {
        return DatabaseRead.GetCollection<T>(typeof(T).Name);
    }
}