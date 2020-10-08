using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Responses
{
    public class ResultResponse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        

        public ResultResponse()
        {
        }

        public static ResultResponse Success(string message)
        {
            return new ResultResponse() {
                code = 1,
                success = true,
                message = message
            };
        }

        public static ResultResponse Fail(string message)
        {
            return new ResultResponse()
            {
                code = 0,
                success = false,
                message = message
            };
        }
    }
}
