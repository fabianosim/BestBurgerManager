using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.Support
{
    /// <summary>
    /// Manipulates hash values. The purpose of this class is to manipulate concurrency on GET an PUT controllers.
    /// </summary>
    public class HashFactory
    {
        /// <summary>
        /// Get the hash for a given object.
        /// </summary>
        /// <param name="model">The object to have its hash extracted.</param>
        /// <returns>A string representing the hash.</returns>
        public static string GetHash(object model)
        {
            string result = string.Empty;
            var json = JsonConvert.SerializeObject(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            using (var hasher = MD5.Create())
            {
                var hash = hasher.ComputeHash(bytes);
                result = BitConverter.ToString(hash);
                result = result.Replace("-", "");
            }

            return result;
        }
    }
}
