using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BestBurgerManager.Business.Extenstions.Exceptions
{
    /// <summary>
    /// Set it to be serializable, so this exception can be marshalled across applications and threads.
    /// </summary>
    [Serializable]
    public class OrderInvalidStatusException : Exception
    {
        /// <summary>
        /// Constructor for this custom exception.
        /// </summary>
        public OrderInvalidStatusException()
            : base()
        { }

        /// <summary>
        /// Creates the exception with a custom message.
        /// </summary>
        /// <param name="message">Exception description</param>
        public OrderInvalidStatusException(String message)
          : base(message)
        {
        }

        /// <summary>
        /// Creates the exception with description and an inner exception.
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public OrderInvalidStatusException(String message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected OrderInvalidStatusException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
