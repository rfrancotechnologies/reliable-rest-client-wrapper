using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReliableRestClient.Exceptions
{
    public class RestException : Exception
    {
        public int HttpCode { get; set; }

        public RestException(int code, string message) : base(message)
        {
            this.HttpCode = code;
        }

        public RestException(int code, string message, Exception detail) : base(message, detail)
        {
            this.HttpCode = code;
        }
    }
}
