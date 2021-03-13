using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Responses
{
    public class ResponseResult<T>
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string debugMessage { get; set; }
        public T data { get; set; }
        

        public ResponseResult()
        {
            success = false;
        }

        public static ResponseResult<T> Success(string message)
        {
            return new ResponseResult<T>() {
                code = 1,
                success = true,
                message = message
            };
        }

        public static ResponseResult<T> Success(string message, T data)
        {
            return new ResponseResult<T>()
            {
                code = 1,
                success = true,
                message = message,
                data = data
            };
        }

        public static ResponseResult<T> Fail(string message, string debugMessage = null)
        {
            return new ResponseResult<T>()
            {
                code = 0,
                success = false,
                message = message,
                debugMessage = debugMessage
            };
        }
    }
}
