using Microsoft.AspNetCore.Http;
using Nlog4.Models.HelpModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.Text;
using System.IO;

namespace Nlog4.Middlewares
{
    public class RequestLoggerMiddleware
    {
        /// <summary>
        /// 中间件回路
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggerMiddleware> _nlog;
        private RequestLogger _logger;


        public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> nlog)
        {
            _next = next;
            _nlog = nlog;
        }

        /// <summary>
        /// 中间件处理步骤
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            _logger = new RequestLogger();
            HttpRequest req = context.Request;
            _logger.ExcuteStartTime = DateTime.Now;

            if (req.Method.ToLower().Equals("post"))
            {
                req.EnableBuffering();

                var stream = req.Body;
                byte[] buffer = new byte[req.ContentLength.Value];
                stream.ReadAsync(buffer, 0, buffer.Length);
                _logger.RequestBody = Encoding.UTF8.GetString(buffer);

                req.Body.Position = 0;
            }

            if (req.Method.ToLower().Equals("get"))
            {
                _logger.RequestBody = req.QueryString.Value;
            }

            var resStream = context.Response.Body;

            using (var resBody = new MemoryStream())
            {
                context.Response.Body = resBody;

                await _next.Invoke(context);

                _logger.ResponseBody = await FormatResponse(context.Response);
                _logger.ExcuteEndTime = DateTime.Now;

                _nlog.LogInformation(_logger.ToString());

                resBody.CopyToAsync(resStream);
            }

        }

        /// <summary>
        /// 格式化返回值
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> FormatResponse(HttpResponse res)
        {
            res.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(res.Body).ReadToEndAsync();
            res.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }


    public static class RequestLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggerMiddleware>();
        }
    }
}
