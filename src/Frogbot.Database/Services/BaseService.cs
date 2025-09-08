using MongoDB.Driver;

namespace Frogbot.Database.Services;

public abstract class BaseService<T>(IMongoDatabase database, string collectionName) where T : class
{
    private readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);

    public virtual async Task InsertAsync(T entity)
    {
        await this._collection.InsertOneAsync(entity);
    }

    public virtual async Task InsertManyAsync(IEnumerable<T> entity)
    {
        await this._collection.InsertManyAsync(entity);
    }

    public virtual Task UpdateAsync(T entity)
    {
        return Task.CompletedTask;
    }

    // protected abstract FilterDefinition<T> Create TODO
}