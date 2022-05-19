using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WhiteBlackList.Web.MiddleWares
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;
        public IPSafeMiddleWare(RequestDelegate next, IOptions<IPList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            var reqIpAdress = context.Connection.RemoteIpAddress;
            // kabul edilecefk Api ler aşağıda tanımlanır
            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAdress)).Any();
            if (isWhiteList)
            {
                //Forbidden 403 dçner
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next(context);
        }
    }
}
