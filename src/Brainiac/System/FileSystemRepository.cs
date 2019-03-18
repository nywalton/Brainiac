using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Brainiac.Contract;

namespace Brainiac.System
{
    public class FileSystemRepository<T, TId> : IRepository<T, TId> where T : IDataItem<TId>
    {
        private static readonly ILogger _log = LogWrapper.Instance.GetLogger<FileSystemRepository<T, TId>>();
        private readonly AppConfig _config;

        private readonly string _filePath;

        /// <summary>
        /// A FileSystemRepo that will set up the local database file from the appsettings.
        /// </summary>
        /// <param name="options"></param>
        public FileSystemRepository(IOptions<AppConfig> options)
        {
            _config = options.Value;
            if (string.IsNullOrWhiteSpace(_config.LocalRepoBaseFileName)) throw new ArgumentException("Using a local file system repository requires a localRepoBaseFileName in the appsettings.");

            _filePath = string.Concat(_config.LocalRepoBaseFileName, $"_{typeof(T).Name}", ".json");
            _log.LogInformation($"Local File DB located at: {_filePath}");

            if (!File.Exists(_filePath))
            {
                FileStream databaseFile = File.Create(_filePath);
                databaseFile.Close();
            }
        }

        /// <summary>
        /// Returns the total number of items in the repo.
        /// </summary>
        /// <returns>The total number of items in the repo.</returns>
        public int Count()
        {
            List<T> data = Load();
            return data.Count;
        }

        /// <summary>
        /// Returns a list of all of the items in the repo.
        /// </summary>
        /// <returns>A list of all of the items in the repo.</returns>
        public List<T> GetAll()
        {
            return Load();
        }

        /// <summary>
        /// Returns an element that matches the specified Id.
        /// </summary>
        /// <param name="id">The Id to search the internal repo for.</param>
        /// <returns>An element that matches the specified Id.</returns>
        public T GetById(TId id)
        {
            List<T> data = Load();
            return data.SingleOrDefault(element => element.Id.Equals(id));
        }

        /// <summary>
        /// Remove the last item from the repo.
        /// </summary>
        public void Remove()
        {
            List<T> data = Load();
            data.RemoveAt(data.Count - 1);
            Save(data);
        }

        /// <summary>
        /// Removes all items from the repo.
        /// </summary>
        public void RemoveAll()
        {
            List<T> data = Load();
            data.RemoveRange(0, data.Count - 1);
            Save(data);
        }

        /// <summary>
        /// Save an entity to the repo.
        /// </summary>
        /// <param name="entity">The entity to save to the repo.</param>
        /// <returns>A string specifying if the entity was updated or added to the repo.</returns>
        public string Save(T entity)
        {
            List<T> data = Load();

            T existingItem = data.SingleOrDefault(item => item.Id.Equals(entity.Id));
            if (existingItem != null)
            {
                existingItem = entity;
            }
            else
            {
                data.Add(entity);
            }

            Save(data);
            return $"Saved record with id = {entity.Id}";
        }

        /// <summary>
        /// 
        /// </summary>
        private List<T> Load()
        {
            string currentData = File.ReadAllText(_filePath);
            return string.IsNullOrWhiteSpace(currentData) ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(currentData);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Save(List<T> data)
        {
            string currentData = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, currentData, Encoding.UTF8);
        }
    }
}
