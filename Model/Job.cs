
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Job
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }
        public IEnumerable<string> CronExpressions { get; set; }
    }
}
