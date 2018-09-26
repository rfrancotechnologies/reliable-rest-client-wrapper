using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReliableRestClient.Exceptions
{
    public class RestTimeoutException : RestException
    {
        public RestTimeoutException(int code, string message) : base(code, message)
        {
        }

        public RestTimeoutException(int code, string message, Exception detail) : base(code, message, detail)
        {
        }
    }
}
