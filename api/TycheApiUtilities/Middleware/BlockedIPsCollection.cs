using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace Tyche.TycheApiUtilities.Middleware
{
    public class BlockedIPsCollection
    {
        private Dictionary<int, HashSet<IPAddress>> storage;

        public BlockedIPsCollection()
        {
            this.storage = new Dictionary<int, HashSet<IPAddress>>();
        }
    }
}