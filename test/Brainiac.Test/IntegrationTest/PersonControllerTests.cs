using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Brainiac.Entity;

namespace Brainiac.Test.IntegrationTest
{
    /// <summary>
    /// Integration tests for PersonController
    /// </summary>
    [TestFixture]
    public class PersonControllerTests
    {
        private TestServer _testServer;
        private HttpClient _client;

        /// <summary>
        /// Set up steps including deleting local persistent storage and setting up the
        /// TestServer and Test WebClient.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // TODO Pull this from app settings
            File.Delete(@"c:/temp/test_localPersonDb_Person.json");

            _testServer = new TestServer(new WebHostBuilder().UseStartup<StartUp>());
            _client = _testServer.CreateClient();
        }

        /// <summary>
        /// Tear down steps including deleting local persistent storage and disposing off the
        /// TestServer and Test WebClient.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // TODO Pull this from app settings
            File.Delete(@"c:/temp/test_localPersonDb_Person.json");

            _testServer.Dispose();
            _client.Dispose();
        }

        /// <summary>
        /// Test saving a person.
        /// </summary>
        /// <param name="host"></param>
        [Test]
        public async Task SavePersonTest()
        {
            Person person = new Person
            {
                Id = 0,
                FirstName = "milla",
                LastName = "maxwell",
                Email = "milla@xillia.com"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("person/save", content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            Assert.That(result.Contains("saved", StringComparison.InvariantCultureIgnoreCase));

            // Remove the test person from the repo
            HttpResponseMessage deleteResponse = await _client.GetAsync("person/remove");
            deleteResponse.EnsureSuccessStatusCode();
            string deleteResult = await deleteResponse.Content.ReadAsStringAsync();
            Assert.That(deleteResult.Contains("removed", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Test saving a person.
        /// </summary>
        [Test]
        public async Task GetPersonsTest()
        {
            Person person = new Person
            {
                Id = 1,
                FirstName = "milla",
                LastName = "maxwell",
                Email = "milla@xillia.com"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("person/save", content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            Assert.That(result.Contains("saved", StringComparison.InvariantCultureIgnoreCase));

            HttpResponseMessage getResponse = await _client.GetAsync("person/all");
            getResponse.EnsureSuccessStatusCode();

            string getResult = await getResponse.Content.ReadAsStringAsync();
            List<Person> retrievedPersons = JsonConvert.DeserializeObject<List<Person>>(getResult);
            Assert.NotNull(retrievedPersons);
            Assert.AreEqual(1, retrievedPersons.Count);
            Assert.AreEqual("milla", retrievedPersons[0].FirstName);
            Assert.AreEqual("maxwell", retrievedPersons[0].LastName);

            // Remove the test person from the repo
            HttpResponseMessage deleteResponse = await _client.GetAsync("person/remove");
            deleteResponse.EnsureSuccessStatusCode();
            string deleteResult = await deleteResponse.Content.ReadAsStringAsync();
            Assert.That(deleteResult.Contains("removed", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Test retrieving a person using basic authorization.
        /// </summary>
        [Test]
        public async Task GetPersonsSecureTest()
        {
            Person person = new Person
            {
                Id = 1,
                FirstName = "milla",
                LastName = "maxwell",
                Email = "milla@xillia.com"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("person/save", content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            Assert.That(result.Contains("saved", StringComparison.InvariantCultureIgnoreCase));

            // Add some authorization for the secure
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "user and pass");
            HttpResponseMessage getResponse = await _client.GetAsync("secure/person/all");
            getResponse.EnsureSuccessStatusCode();

            string getResult = await getResponse.Content.ReadAsStringAsync();
            List<Person> retrievedPersons = JsonConvert.DeserializeObject<List<Person>>(getResult);
            Assert.NotNull(retrievedPersons);
            Assert.AreEqual(1, retrievedPersons.Count);
            Assert.AreEqual("milla", retrievedPersons[0].FirstName);
            Assert.AreEqual("maxwell", retrievedPersons[0].LastName);

            // Remove the test person from the repo
            HttpResponseMessage deleteResponse = await _client.GetAsync("person/remove");
            deleteResponse.EnsureSuccessStatusCode();
            string deleteResult = await deleteResponse.Content.ReadAsStringAsync();
            Assert.That(deleteResult.Contains("removed", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Test retrieving a person using the secure endpoint without using basic authorization.
        /// </summary>
        [Test]
        public async Task GetPersonsSecureNoAuthTest()
        {
            Person person = new Person
            {
                Id = 1,
                FirstName = "milla",
                LastName = "maxwell",
                Email = "milla@xillia.com"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("person/save", content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            Assert.That(result.Contains("saved", StringComparison.InvariantCultureIgnoreCase));

            // No authorization should throw an HttpException
            _client.DefaultRequestHeaders.Clear();
            HttpResponseMessage getResponse = await _client.GetAsync("secure/person/all");
            Assert.AreEqual(HttpStatusCode.Unauthorized, getResponse.StatusCode);
            
            // Remove the test person from the repo
            HttpResponseMessage deleteResponse = await _client.GetAsync("person/remove");
            deleteResponse.EnsureSuccessStatusCode();
            string deleteResult = await deleteResponse.Content.ReadAsStringAsync();
            Assert.That(deleteResult.Contains("removed", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
