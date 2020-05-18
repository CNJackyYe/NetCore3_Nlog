using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nlog4.Models.HelpModels
{
    public class RequestLogger
    {
        public object RequestBody { get; set; }
        public object ResponseBody { get; set; }
        public DateTime ExcuteStartTime { get; set; }
        public DateTime ExcuteEndTime { get; set; }
        public override string ToString()
        {
            var res = JsonConvert.SerializeObject(new RequestLoggerFormat
            {
                RequestBody = this.RequestBody,
                ResponseBody = this.ResponseBody,
                ExcuteEndTime = this.ExcuteStartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ExcuteStartTime= this.ExcuteEndTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
            });

            return res;
        }


        private class RequestLoggerFormat
        {
            public object RequestBody { get; set; }
            public object ResponseBody { get; set; }
            public string ExcuteStartTime { get; set; }
            public string ExcuteEndTime { get; set; }
        }
    }

    


}





