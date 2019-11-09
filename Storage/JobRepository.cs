using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Newtonsoft.Json;
using Model;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Globalization;

namespace Storage
{
    public class JobRepository : IJobRepository
    {
        private readonly CloudTable _table;
        private readonly string separator = " -- ";
        private JobRepository()
        {
            TypeAdapterConfig<Job, JobEntity>.NewConfig()
                .Map(dest => dest.CompanyId, src => src.CompanyId)
                .Map(dest => dest.CronExpressions, src => string.Join(separator, src.CronExpressions))
                .Map(dest => dest.JsonData, src => src.JsonData)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PartitionKey, src => src.CompanyId)
                .Map(dest => dest.RowKey, src => src.Name);

            TypeAdapterConfig<JobEntity, Job>.NewConfig()
                .Map(dest => dest.CompanyId, src => src.CompanyId)
                .Map(dest => dest.CronExpressions, src => src.CronExpressions.Split(separator, StringSplitOptions.None))
                .Map(dest => dest.JsonData, src => src.JsonData)
                .Map(dest => dest.Name, src => src.Name);

            string connectionString = "PUT YOUR CONNECTION STRING HERE";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference("schedule-job");
            _table.CreateIfNotExistsAsync().Wait();
        }

        public static IJobRepository Instance { get; } = new JobRepository();

        public async Task<Job> AddOrUpdate(Job job)
        {
            JobEntity entity = job.Adapt<JobEntity>();
            TableOperation operation = TableOperation.InsertOrReplace(entity);
            TableResult result = await _table.ExecuteAsync(operation).ConfigureAwait(false);
            JobEntity storedEntity = (JobEntity)result.Result;

            return storedEntity.Adapt<Job>();
        }

        public async Task<Job> GetJob(string companyId, string jobName)
        {
            TableOperation operation = TableOperation.Retrieve<JobEntity>(companyId, jobName);
            TableResult result = await _table.ExecuteAsync(operation).ConfigureAwait(false);

            return result.Result.Adapt<Job>();
        }

        public async Task<IEnumerable<Job>> GetJobs(string companyId = null)
        {
            TableQuerySegment<JobEntity> result;
            TableQuery<JobEntity> query = new TableQuery<JobEntity>();
            if (string.IsNullOrWhiteSpace(companyId))
                result = await _table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false);
            else
            {
                string filter = TableQuery.GenerateFilterCondition(nameof(JobEntity.PartitionKey), QueryComparisons.Equal, companyId);
                query = query.Where(filter);
                result = await _table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false);
            }

            return result.Results.Select(entity => entity.Adapt<Job>());
        }
    }
}
