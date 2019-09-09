using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsoni.LogManager.Standard.ExceptionTypes
{
    [Serializable]
    public class LogMangerException : Exception
    {
        public LogMangerException(string key) : base(key)
        {
        }

        public LogMangerException(string key, Exception exception) : base(key, exception)
        {
        }
    }
}
