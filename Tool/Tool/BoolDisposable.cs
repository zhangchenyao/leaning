using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    class BoolDisposable : IDisposable
    {
        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
        }
        ~BoolDisposable()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool dispoing)
        {
            if (disposed)
            {
                return;
            }
            if (dispoing)
            { 
            
            }
        }
    }
}
