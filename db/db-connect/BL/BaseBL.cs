using System;
using System.Collections.Generic;
using System.Text;
using AccessCore.Repository;

namespace DbConnect.BL
{
    /// <summary>
    /// Base class for Business Logic
    /// </summary>
    public class BaseBL
    {
        /// <summary>
        /// Data manager
        /// </summary>
        protected readonly DataManager dm;

        /// <summary>
        /// Business logic type
        /// </summary>
        protected readonly BlType blType;

        /// <summary>
        /// Creates new instance of <see cref="BaseBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        /// <param name="blType">Business Logic type.</param>
        public BaseBL(DataManager dm, BlType blType = BlType.BaseBL)
        {
            this.dm = dm;
            this.blType = blType;
        }
    }
}