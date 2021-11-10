using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZoomitTest.Model
{
    public class AccessLog
    {
        public string Ip { get; set; }
        public List<AccessLogItem> Logs { get; set; }
    }
    public class AccessLogItem
    {
        public string Url { get; set; }
        public DateTimeOffset LastRequest { get; set; }
    }

}