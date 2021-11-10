using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ZoomitTest.Dto;
using ZoomitTest.Model;

namespace ZoomitTest.Infrastructure.Extensions
{
    public static class AppExtensions
    {
        public static long GetDuration(this string durationType)
        {
            return durationType.ToLower() switch
            {
                "sec" => 1,
                "min" => 60,
                "hr" => 3600,
                _ => throw new ArgumentException("invalid durationType!!!!"),
            };
        }
        public static LimitDto SplitLimitAndDuration(this string entry)
        {
            string[] values = entry.Split("/");
            return new LimitDto
            {
                Duration = values[1].GetDuration(),
                Limit = Int64.Parse(values[0])
            };
        }
    }
}