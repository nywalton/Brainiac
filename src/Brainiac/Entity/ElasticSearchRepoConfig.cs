namespace Brainiac.Entity
{
    public class ElasticSearchRepoConfig
    {
        /// <summary>
        /// The ElasticSearch server.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// The Index to use.
        /// </summary>
        public string Index { get; set; }
    }
}
