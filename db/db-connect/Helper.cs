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
        /// <typeparam name="TOut">Type of output.</typeparam>
        /// <param name="blFunction">BL function.</param>
        /// <returns>Handler</returns>
        internal static Func<object, Task<object>> CostructHandler<TIn, TOut>(Func<TIn, Task<TOut>> blFunction)
            where TIn : class
        {
            return async input => await blFunction(input as TIn);
        }
    }
}
