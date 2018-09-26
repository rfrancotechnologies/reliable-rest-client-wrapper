using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReliableRestClient.Exceptions
{
    public class RestServerSideException : RestException
    {
        public RestServerSideException(int code, string message) : base(code, message)
        {
        }

        public RestServerSideException(int code, string message, Exception detail) : base(code, message, detail)
        {

        }

    }
}
