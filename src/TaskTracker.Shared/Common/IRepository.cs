namespace TaskTracker.Kernel.Common;

public interface IRepository<T> : IUnitOfWork
{
    /// <summary>  
    /// Retrieves an IQueryable for querying the entities of type T.  
    /// </summary>  
    IQueryable<T> Query();

    /// <summary>  
    /// Retrieves an IQueryable for querying the entities of type T.  
    /// </summary> 
    Task<IQueryable<T>> QueryAsync();

    /// <summary>  
    /// Retrieves an entity of type T by its unique identifier asynchronously.  
    /// </summary>  
    /// <param name="id">The unique identifier of the entity.</param>  
    /// <param name="cancellationToken">A token to cancel the operation.</param>  
    /// <returns>The entity if found, otherwise null.</returns>  
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>  
    /// Adds a single entity of type T to the repository.  
    /// </summary>  
    /// <param name="entity">The entity to add.</param>  
    void Add(T entity);

    /// <summary>  
    /// Adds multiple entities of type T to the repository.  
    /// </summary>  
    /// <param name="entities">The collection of entities to add.</param>  
    void Add(IEnumerable<T> entities);

    /// <summary>  
    /// Updates a single entity of type T in the repository.  
    /// </summary>  
    /// <param name="entity">The entity to update.</param>  
    void Update(T entity);

    /// <summary>  
    /// Updates multiple entities of type T in the repository.  
    /// </summary>  
    /// <param name="entities">The collection of entities to update.</param>  
    void Update(IEnumerable<T> entities);

    /// <summary>  
    /// Deletes a single entity of type T from the repository.  
    /// </summary>  
    /// <param name="entity">The entity to delete.</param>  
    void Delete(T entity);

    /// <summary>  
    /// Deletes multiple entities of type T from the repository.  
    /// </summary>  
    /// <param name="entities">The collection of entities to delete.</param>  
    void Delete(IEnumerable<T> entities);
}
