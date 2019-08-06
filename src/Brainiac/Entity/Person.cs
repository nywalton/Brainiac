using Brainiac.Contract;

namespace Brainiac.Entity
{
    /// <summary>
    /// A simple Person class.
    /// </summary>
    public class Person : IDataItem 
    {
        /// <summary>
        /// The Id of the person
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

    }
}
