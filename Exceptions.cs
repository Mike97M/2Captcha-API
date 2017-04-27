using System;

namespace API2Captcha
{
    public class BalanceException : Exception
    {
        public BalanceException(string message, Exception inner) : base(message, inner)
        {

        }
    }
    public class ResponseException : Exception
    {
        public ResponseException(string message) : base(message)
        {

        }
    }
}
