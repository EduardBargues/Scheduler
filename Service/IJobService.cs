using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Service
{
    public interface IJobService
    {
        public Task<Job> AddOrUpdate(Job job);
        Task<IEnumerable<Job>> GetJobs(string companyId, string name);
    }
}