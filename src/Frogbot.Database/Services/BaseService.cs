using MongoDB.Driver;
using NetCord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frogbot.Database;

public abstract class BaseService<T>(IMongoDatabase database, string collectionName) where T : class
{
    private readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);

    public virtual async Task InsertUserAsync(T entity)
    {
        await this._collection.InsertOneAsync(entity);
    }

    public virtual async Task InsertUsersAsync(List<T> entity)
    {
        await this._collection.InsertManyAsync(entity);
    }

    public virtual Task UpdateUserAsync(T entity)
    {
        return Task.CompletedTask;
    }

    // protected abstract FilterDefinition<T> Create TODO
}