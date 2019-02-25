using System;
using System.Threading.Tasks;

namespace DbConnect
{
    /// <summary>
    /// Helper for server
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Constructs handler from BL function
        /// </summary>
        /// <typeparam name="TIn">Type of input.</typeparam>
        /// <param name="blFunction">BL function.</param>
        /// <returns>Handler</returns>
        internal static Func<object, Task<DbResponse>> CostructHandler<TIn>(Func<TIn, Task<DbResponse>> blFunction)
            where TIn : class
        {
            return async input => await blFunction(input as TIn);
        }
    }
}
