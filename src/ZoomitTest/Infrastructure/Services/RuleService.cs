using System;
using System.Threading.Tasks;
using ZoomitTest.Dto;
using ZoomitTest.Infrastructure.Extensions;
using ZoomitTest.Infrastructure.Repositories;
using ZoomitTest.Model;

namespace ZoomitTest.Infrastructure.Services
{
    public interface IRuleService
    {
        Task AddOneAsync(RuleEntryDto ruleEntry);
        Task<Rule> GetOneAsync(string url);
    }

    public class RuleService : IRuleService
    {
        private readonly IRedisRepository _redisRepository;

        public RuleService(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public async Task AddOneAsync(RuleEntryDto ruleEntry)
        {
            var limit = ruleEntry.Duration.SplitLimitAndDuration();
            var rule = new Rule
            {
                Url = ruleEntry.Url,
                Duration = limit.Duration,
                Limit = limit.Limit
            };
            await _redisRepository.AddAsync(rule.Url, rule);
        }
        public async Task<Rule> GetOneAsync(string url)
        {
            var rule = await _redisRepository.GetAsync<Rule>(url);
            if (rule is null)
            {
                while (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        url = url[..url.LastIndexOf("/")];
                        rule = await _redisRepository.GetAsync<Rule>(url + "/*");
                        if (rule is not null)
                            break;
                    }
                    catch (System.Exception)
                    {
                        throw new Exception("rule not found!!!"); 
                    }
                }
            }
            return rule;
        }
    }
}