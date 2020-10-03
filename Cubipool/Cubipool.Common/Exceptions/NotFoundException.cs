using System;
using System.Net;
using Cubipool.Common.Constants;

namespace Cubipool.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
        : base(message)
        {
            Data.Add(Errors.DictErrorKey, HttpStatusCode.NotFound);
        }
    }
}
