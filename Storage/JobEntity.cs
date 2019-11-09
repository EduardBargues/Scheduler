using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace Storage
{
    internal class JobEntity : TableEntity
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }
        public string CronExpressions { get; set; }
    }
}
