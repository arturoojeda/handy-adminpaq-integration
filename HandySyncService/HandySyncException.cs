using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService
{
    class HandySyncException : Exception
    {
        public HandySyncException()
        { }

        public HandySyncException(string message) : base(message)
        { }
    }
}


