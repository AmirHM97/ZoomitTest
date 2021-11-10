using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZoomitTest.Dto;
using ZoomitTest.Infrastructure.Repositories;
using ZoomitTest.Model;

namespace ZoomitTest.Infrastructure.Services
{
    public interface IAccessLogManagementService
    {
        //Task<AccessLog> AddAccessLog(CheckAccessDto checkAccessDto);
        Task<bool> CheckAccessLog(CheckAccessDto checkAccessDto);
        Task<AccessLog> GetUserAccessLogs(string ip);
        Task<AccessLog> UpdateAccessLog(CheckAccessDto checkAccessDto);
    }

    public class AccessLogManagementService : IAccessLogManagementService
    {
        private readonly IRedisRepository _redisRepository;
        private readonly IRuleService _ruleService;

        public AccessLogManagementService(IRedisRepository redisRepository, IRuleService ruleService)
        {
            _redisRepository = redisRepository;
            _ruleService = ruleService;
        }
        //public async Task<AccessLog> AddAccessLog(CheckAccessDto checkAccessDto)
        //{
        //    var access = new AccessLog
        //    {
        //        Ip = checkAccessDto.Ip,
        //        Logs = new List<AccessLogItem>{
        //            new  AccessLogItem{LastRequest=DateTimeOffset.UtcNow,
        //            Url=checkAccessDto.Url}
        //        }
        //    };
        //    await _redisRepository.AddAsync(access.Ip, access);
        //    return access;
        //}
        public async Task<AccessLog> UpdateAccessLog(CheckAccessDto checkAccessDto)
        {
            var logs = await GetUserAccessLogs(checkAccessDto.Ip) ?? new() ;
            logs.Ip = checkAccessDto.Ip;
            logs.Logs.Add(new AccessLogItem
            {
                LastRequest = DateTimeOffset.UtcNow,
                Url = checkAccessDto.Url
            });
            await _redisRepository.AddAsync(checkAccessDto.Ip, logs);
         
            return logs;
        }
        public async Task<AccessLog> GetUserAccessLogs(string ip)
        {
            var logs = await _redisRepository.GetAsync<AccessLog>(ip);
            return logs;
        }
        public async Task<bool> CheckAccessLog(CheckAccessDto checkAccessDto)
        {
            var logs = await UpdateAccessLog(checkAccessDto);
            var rule = await _ruleService.GetOneAsync(checkAccessDto.Url);
            var reqCount = logs.Logs.Count(w => w.Url.Equals(checkAccessDto.Url) && w.LastRequest >= DateTimeOffset.UtcNow.AddSeconds(rule.Duration * -1));
            return reqCount <= rule.Limit ;
        }
    }
}