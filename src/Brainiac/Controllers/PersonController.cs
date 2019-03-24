using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Brainiac.Contract;
using Brainiac.Entity;

namespace Brainiac.Controllers
{
    /// <summary>
    /// A controller that exposes the API endpoints for the Person class.
    /// </summary>
    public class PersonController : Controller
    {
        private static readonly ILogger _log = LogWrapper.Instance.GetLogger<PersonController>();

        private readonly IRepository<Person, int> _repository;

        /// <summary>
        /// Constructor that initialize the correcty type of IRepository
        /// </summary>
        /// <param name="personRepository"></param>
        public PersonController(IRepository<Person, int> personRepository)
        {
            _repository = personRepository;
        }

        /// <summary>
        /// Retrieve a Person from persistent storage.
        /// </summary>
        /// <param name="id">The Id of the person to retrieve.</param>
        /// <returns>The Person with the matching Id, or null if there is no person with the specified Id.</returns>
        [HttpGet("person/get/{id}")]
        public Person GetPerson(int id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Remove a person from persistent storage.
        /// </summary>
        /// <returns></returns>
        [HttpGet("person/remove")]
        public string RemovePerson()
        {
            _repository.Remove();
            return $"Last person removed. Total count: {_repository.Count()}";
        }

        /// Remove all persons from persistent storage.
        /// </summary>
        /// <returns></returns>
        [HttpGet("person/removeAll")]
        public string RemoveAllPersons()
        {
            _repository.Remove();
            return $"All persons removed. Total count: {_repository.Count()}";
        }


        /// <summary>
        /// Retrieve all persons from persistent storage.
        /// </summary>
        /// <returns>A List of all persons in persistent storage.</returns>
        [HttpGet("person/all")]
        public List<Person> GetPersons()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Add a person to persistent storage.
        /// </summary>
        /// <param name="person">The person to add to persistent storage.</param>
        /// <returns></returns>
        [HttpPost("person/save")]
        public string SavePerson([FromBody]Person person)
        {
            _log.LogInformation($"Person: {person}");

            return _repository.Save(person);
        }
    }
}
