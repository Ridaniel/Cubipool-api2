using System;
using System.Net;
using Cubipool.Common.Constants;

namespace Cubipool.Common.Exceptions
{
    public class InternalServerError : Exception
    {
        public InternalServerError(string message)
        : base(message)
        {
            Data.Add(Errors.DictErrorKey, HttpStatusCode.InternalServerError);
        }
    }
}
