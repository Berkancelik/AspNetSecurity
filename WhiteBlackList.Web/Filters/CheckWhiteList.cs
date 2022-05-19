using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using WhiteBlackList.Web.MiddleWares;

namespace WhiteBlackList.Web.Filters
{
    // filterlar ile birlikte gelen istekleri controller lara girmeden yakalarız
    public class CheckWhiteList: ActionFilterAttribute
    {
        private readonly IPList _ipList;
        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var reqIpAdress = context.HttpContext.Connection.RemoteIpAddress;
            // Any() var ise true yok ise false dönecektir
            var isWhileList = _ipList.WhiteList.Where(x=> IPAddress.Parse(x).Equals(reqIpAdress)).Any();
            if (isWhileList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
