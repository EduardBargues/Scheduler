using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Quartz;
using Quartz.Impl;
using Storage;

namespace Service
{
    public class JobService : IJobService
    {
        private readonly IScheduler scheduler;

        private JobService()
        {
            scheduler = new StdSchedulerFactory().GetScheduler().Result;
        }

        public static IJobService Instance { get; } = new JobService();

        public async Task<Job> AddOrUpdate(Job job)
        {
            if (job.CronExpressions.Any(ce => !CronExpression.IsValidExpression(ce)))
                throw new Exception("Invalid cron expression");

            foreach (string cronExpression in job.CronExpressions)
            {
                IJobDetail detail = BuildJobDetail(job.JsonData);
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();
                scheduler.ScheduleJob(detail, trigger);
            }

            Job storedJob = await JobRepository.Instance.AddOrUpdate(job).ConfigureAwait(false);

            return storedJob;
        }

        public async Task<IEnumerable<Job>> GetJobs(string companyId, string name)
            => string.IsNullOrWhiteSpace(name)
                ? await JobRepository.Instance.GetJobs(companyId).ConfigureAwait(false)
                : (await JobRepository.Instance.GetJob(companyId, name).ConfigureAwait(false)).ToEnumerable();

        private IJobDetail BuildJobDetail(string data)
            => JobBuilder.Create(typeof(JobExecution))
                .UsingJobData(new JobDataMap((IDictionary<string, object>)new Dictionary<string, object>() { { JobNames.JobData, data } }))
                .Build();
    }
}
