using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nlog4.Models.HelpModels
{
    public static class HttpContext
    {
        public static IServiceCollection serviceCollection;

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                object factory = serviceCollection.BuildServiceProvider().GetService(typeof(IHttpContextAccessor));
                Microsoft.AspNetCore.Http.HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

    }
}
