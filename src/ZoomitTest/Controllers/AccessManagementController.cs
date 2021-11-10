using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZoomitTest.Dto;
using ZoomitTest.Infrastructure.Services;

namespace ZoomitTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccessManagementController : ControllerBase
    {
        private readonly IAccessLogManagementService _accessLogManagementService;

        public AccessManagementController(IAccessLogManagementService accessLogManagementService)
        {
            _accessLogManagementService = accessLogManagementService;
        }

        [HttpPost]
        public async Task<ActionResult> CheckAccessAsync(CheckAccessDto checkAccessDto)
        {
            var res = await _accessLogManagementService.CheckAccessLog(checkAccessDto);
            if (res)
                return Ok();
            else
                return new ContentResult { Content = "THROTTLED", StatusCode = 429 };


        }
        [HttpGet]
        public async Task<ActionResult> test(string url)
        {
            var list = new List<string>();
            while (!string.IsNullOrEmpty(url))
            {
                try
                {
                    url = url.Substring(0, url.LastIndexOf("/"));
                    list.Add(url);

                }
                catch (System.Exception)
                {
                    break;
                }
                // url = url[..(url.LastIndexOf("/"))];
            }
            return Ok(list);


        }

    }
}