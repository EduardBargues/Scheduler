using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Model;
using System.Net.Mail;
using System.Net;

namespace Service
{
    internal class JobExecution : IJob
    {
        class JobEvent
        {
            public string JsonData { get; set; }
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobEvent ev = new JobEvent()
            {
                JsonData = (string)context.Get(JobNames.JobData)
            };
            // Launch event to azure with json data
            return Task.CompletedTask;
        }
    }
}
