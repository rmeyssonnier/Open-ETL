using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PampaSoft.Data.Etl.Api.DbContext;
using PampaSoft.Data.Etl.Api.Repositories;

namespace PampaSoft.Data.Etl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PipelineController
    {
        private PipelineRepository _pipelineRepository = null;
        
        public PipelineController(MainDbContext context)
        {
            this._pipelineRepository = new PipelineRepository(context);
        }
        
        [HttpPost]
        public async Task<Pipeline> CreatePipeline([FromBody] Pipeline pipeline)
        {
            return await _pipelineRepository.Create(pipeline);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<Pipeline> ReadPipeline([FromRoute] int id)
        {
            return await _pipelineRepository.Read(id);
        }
        
        [HttpGet]
        [Route("all")]
        public async Task<ICollection<Pipeline>> ReadAllPipeline()
        {
            return await _pipelineRepository.ReadAll();
        }
        
        [HttpPatch]
        public async Task PatchPipeline([FromBody] Pipeline pipeline)
        {
            await _pipelineRepository.Update(pipeline);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeletePipeline([FromRoute] int id)
        {
            await this._pipelineRepository.Delete(id);
        }
    }
}