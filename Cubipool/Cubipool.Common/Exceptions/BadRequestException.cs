using System;
using System.Net;
using Cubipool.Common.Constants;

namespace Cubipool.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
        : base(message)
        {
            Data.Add(Errors.DictErrorKey, HttpStatusCode.BadRequest);
        }
    }
}
