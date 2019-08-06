using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brainiac.Contract;
using Brainiac.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace Brainiac.System
{
    public class ElasticSearchRepository<T> : Contract.IRepository<T>
        where T : class, IDataItem
    {
        private static readonly ILogger _log = LogWrapper.Instance.GetLogger<ElasticSearchRepository<T>>();

        private readonly ElasticSearchRepoConfig _config;
        private readonly ElasticClient _elasticClient;

        public ElasticSearchRepository(IOptions<ElasticSearchRepoConfig> options)
        {
            _config = options.Value;

            // todo nwalton add in settings for authentication
            ConnectionSettings connectionSettings = new ConnectionSettings(new Uri(_config.Server));
            _elasticClient = new ElasticClient(connectionSettings);
        }

        public int Count()
        {
            var countRequest = new CountRequest(_config.Index);
            var countResponse = _elasticClient.Count(countRequest);

            return (int)countResponse.Count;
        }

        public List<T> GetAll()
        {
            var searchRequest = new SearchRequest<T>(Indices.Index(_config.Index));
            //{
            //    From = 0,
            //    Size = 10
            //};

            var searchResponse = _elasticClient.Search<T>(searchRequest);
            return searchResponse.Documents.Cast<T>().ToList();
        }

        public T GetById(object id)
        {
            var searchResponse = _elasticClient.Search<T>(searchRequest => searchRequest
                .Index(_config.Index)
                .Query(query => query.Term("Id", id))
            );
            return searchResponse.Documents.FirstOrDefault();
        }

        public void Remove(object id)
        {
            string idString = Convert.ToString(id);
            var deleteRequest = new DeleteRequest(_config.Index, idString);
            _elasticClient.Delete(deleteRequest);
        }

        public void RemoveAll()
        {
            _elasticClient.Indices.Delete(_config.Index);
        }

        public string Save(T entity)
        {
            var existsResponse = _elasticClient.Indices.Exists(_config.Index);
            if (!existsResponse.Exists)
            {
                _elasticClient.Indices.Create(_config.Index);
            }

            IndexResponse indexResponse = _elasticClient.Index(entity, indexRequest => indexRequest.Index(_config.Index));
            return $"Saved record with id = {indexResponse.Id}";
        }
    }
}
