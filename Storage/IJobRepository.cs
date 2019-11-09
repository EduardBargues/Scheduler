using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Storage
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetJobs(string companyId = null);
        Task<Job> GetJob(string companyId, string jobName);
        Task<Job> AddOrUpdate(Job job);
    }
}
