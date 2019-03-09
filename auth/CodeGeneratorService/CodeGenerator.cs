using System;
using System.Security.Cryptography;

namespace CodeGeneratorService
{
    /// <summary>
    /// Code generator
    /// </summary>
    public class CodeGenerator : IDisposable
    {
        /// <summary>
        /// Random number generator service provider
        /// </summary>
        private readonly RNGCryptoServiceProvider rNGCryptoServiceProvider;

        /// <summary>
        /// Creates new instance of <see cref="CodeGenerator"/>
        /// </summary>
        public CodeGenerator()
        {
            this.rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Generates verification key.
        /// </summary>
        /// <param name="length">Length of key</param>
        /// <returns>Verification key</returns>
        public string GenerateVerifyKey(int length)
        {
            // buffer for storing random bytes
            var buffer = new byte[length];

            // getting random bytes
            this.rNGCryptoServiceProvider.GetBytes(buffer);

            // converting to string
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Disposes code generator
        /// </summary>
        public void Dispose()
        {
            this.rNGCryptoServiceProvider.Dispose();
        }
    }
}