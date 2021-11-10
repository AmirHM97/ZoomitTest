using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZoomitTest.Dto;
using ZoomitTest.Infrastructure.Services;

namespace ZoomitTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RuleManagementController : ControllerBase
    {
        private readonly IRuleService _ruleService;

        public RuleManagementController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpPost]
        public async Task<ActionResult> AddRuleAsync(RuleEntryDto ruleEntryDto)
        {
            await _ruleService.AddOneAsync(ruleEntryDto);
            return Ok();
        }
        
    }
}