using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    class LockHelper
    {
        private static readonly ReaderWriterLockSlim clientIdListLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim clientListLock = new ReaderWriterLockSlim();
        private static readonly object networkWriteLock = new object();
        public static ReaderWriterLockSlim ClientListLock
        {
            get
            {
                return clientListLock;
            }
        }
        public static ReaderWriterLockSlim ClientIdListLock
        {
            get
            {
                return clientIdListLock;
            }
        }

        public static object NetworkWriteLock
        {
            get
            {
                return networkWriteLock;
            }
        }
    }


}
