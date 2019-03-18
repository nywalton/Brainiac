using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Brainiac.Attribute;
using Brainiac.Contract;
using Brainiac.Entity;

namespace Brainiac.Controllers
{
    /// <summary>
    /// A SecurePersonController adds a basic implementation of authentication filter achieved with an attribute.
    /// </summary>
    [ServiceFilter(typeof(AuthenticationFilterAttribute))]
    public class SecurePersonController
    {
        private readonly IRepository<Person, int> _repository;

        /// <summary>
        /// Constructor that initialize the correcty type of IRepository
        /// </summary>
        /// <param name="personRepository"></param>
        public SecurePersonController(IRepository<Person, int> personRepository)
        {
            _repository = personRepository;
        }

        /// <summary>
        /// Retrieve all persons with a basic level of authentication.
        /// </summary>
        /// <returns>A list of Persons.</returns>
        [HttpGet("secure/person/all")]
        public List<Person> GetPersons()
        {
            return _repository.GetAll();
        }
    }
}
