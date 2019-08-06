using System.Collections.Generic;
using System.Linq;
using Brainiac.Contract;

namespace Brainiac.System
{
    /// <summary>
    /// An implementation for a local memory repository.
    /// </summary>
    /// <typeparam name="T">Type stored in the repository.</typeparam>
    /// <typeparam name="TId">Type of the unique identifier for the type stored in the repo.</typeparam>
    public class LocalMemoryRepository<T> : IRepository<T> 
        where T : class, IDataItem
    {
        private static Dictionary<object, T> _data = new Dictionary<object, T>();

        /// <summary>
        /// Returns the total number of items in the repo.
        /// </summary>
        /// <returns>The total number of items in the repo.</returns>
        public int Count()
        {
            return _data.Count;
        }

        /// <summary>
        /// Returns a list of all of the items in the repo.
        /// </summary>
        /// <returns>A list of all of the items in the repo.</returns>
        public List<T> GetAll()
        {
            return _data.Values.ToList();
        }

        /// <summary>
        /// Returns an element that matches the specified Id.
        /// </summary>
        /// <param name="id">The Id to search the internal repo for.</param>
        /// <returns>An element that matches the specified Id.</returns>
        public T GetById(object id)
        {
            return _data[id];
        }

        /// <summary>
        /// Remove the item with the given Id from the repo.
        /// </summary>
        public void Remove(object id)
        {
            if (_data.ContainsKey(id))
            {
                _data.Remove(id);
            }
        }

        /// <summary>
        /// Remove the last item from the repo.
        /// </summary>
        public void RemoveAll()
        {
            _data.Clear();
        }

        /// <summary>
        /// Save an entity to the repo.
        /// </summary>
        /// <param name="entity">The entity to save to the repo.</param>
        /// <returns>A string specifying if the entity was updated or added to the repo.</returns>
        public string Save(T entity)
        {
            if (_data.ContainsKey(entity.Id))
            {
                _data[entity.Id] = entity;
            }
            else
            {
                _data.Add(entity.Id, entity);
            }

            return $"Saved record with id = {entity.Id}";
        }
    }
}
