using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        [HttpGet()]
        public async Task<ObjectResult> GetByCompany(string companyId, string name)
            => Ok(await JobService.Instance.GetJobs(companyId, name).ConfigureAwait(false));

        [HttpPut()]
        public async Task<ObjectResult> AddOrUpdate([FromBody] Job job)
            => Ok(await JobService.Instance.AddOrUpdate(job).ConfigureAwait(false));
    }
}
