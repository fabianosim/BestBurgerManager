using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses
{
    /// <summary>
    /// Handles the HTTP responses to the client.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// The HTTP Status Code.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// A Json Object contianing the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }


        /// <summary>
        /// Sets a response and passes a string as response message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">The string message.</param>
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return "Resource not found";
                case 500:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }
    }
}
