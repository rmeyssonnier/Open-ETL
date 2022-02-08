using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PampaSoft.Data.Etl.Api.Models;
using PampaSoft.Data.Etl.Api.Repository;
using PampaSoft.Data.Etl.Api.Models.ActionConfigurations;

namespace PampaSoft.Data.Etl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EtlFlowController : ControllerBase
    {
        private readonly ILogger<EtlFlowController> _logger;

        public EtlFlowController(ILogger<EtlFlowController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<bool> CreateNewFlow([FromBody] EtlFlowCommand command)
        {
            EtlCommandBuilder etlCommandBuilder = new EtlCommandBuilder(command);
            etlCommandBuilder.Deserialize();
            await etlCommandBuilder.ActionFlow.Execute();
            
            return true;
        }
        
    }
}