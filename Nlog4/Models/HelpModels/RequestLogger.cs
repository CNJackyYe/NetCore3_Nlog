using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Nlog4.Models.HelpModels
{
    public class RequestLogger
    {
        private readonly IHttpContextAccessor _accessor;
        public RequestLogger(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public object RequestBody { get; set; }
        public object ResponseBody { get; set; }
        public DateTime ExcuteStartTime { get; set; }
        public DateTime ExcuteEndTime { get; set; }
        public string UserIP { get; set; }

        public override string ToString()
        {
            var res = JsonConvert.SerializeObject(new RequestLoggerFormat
            {
                RequestBody = this.RequestBody,
                ResponseBody = this.ResponseBody,
                ExcuteEndTime = this.ExcuteStartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ExcuteStartTime = this.ExcuteEndTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                UserIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
            });
            res = res.Replace("\"{", "{");
            res = res.Replace("}\"", "}");
            res = res.Replace("\"[", "[");
            res = res.Replace("]\"", "]");
            res = res.Replace("\\\"", "\"");
            res = res.Replace("\\r\\n", "");
            return res;
        }


        private class RequestLoggerFormat
        {
            public object RequestBody { get; set; }
            public object ResponseBody { get; set; }
            public string ExcuteStartTime { get; set; }
            public string ExcuteEndTime { get; set; }
            public string UserIP { get; set; }
        }
    }

    


}





