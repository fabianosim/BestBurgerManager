using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses
{
    /// <summary>
    /// Class to handle OK responses to the client.
    /// </summary>
    public class ApiOkResponse : ApiResponse
    {
        /// <summary>
        /// The result object that is part of the response.
        /// </summary>
        public object Result { get; }

        /// <summary>
        /// Prepare a HTTP 200 code response to the client with a message.
        /// </summary>
        /// <param name="result">The result object from the called methods.</param>
        public ApiOkResponse(object result)
            : base(200)
        {
            Result = result;
        }
    }
}
