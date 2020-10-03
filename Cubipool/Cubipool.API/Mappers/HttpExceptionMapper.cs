using Cubipool.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;

namespace Cubipool.API
{
    public class HttpExceptionMapper
    {
        public static IActionResult ToHttpActionResult(Exception ex)
        {
            if (ex.Data.Contains(Errors.DictErrorKey))
            {
                var httpStatusCode = (HttpStatusCode?)ex.Data[Errors.DictErrorKey];

                if (httpStatusCode.HasValue)
                {
                    var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        code = (int)httpStatusCode,
                        message = ex.Message
                    });

                    var httpActionResult = new ContentResult()
                    {
                        StatusCode = (int)httpStatusCode,
                        Content = serializedObject,
                        ContentType = MediaTypeNames.Application.Json,
                    };

                    return httpActionResult;
                }
            }

            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            var noStatusCodeError = new ContentResult()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Content = ex.Message,
                ContentType = MediaTypeNames.Application.Json,
            };
            return noStatusCodeError;
        }
    }
}