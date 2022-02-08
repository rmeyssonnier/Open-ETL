using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PampaSoft.Data.Etl.Api.DbContext;

namespace PampaSoft.Data.Etl.Api.Repositories
{
    public class PipelineRepository
    {
        private MainDbContext _context = null;
        public PipelineRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task<Pipeline> Create(Pipeline pipeline)
        {
            pipeline = (await this._context.Pipelines.AddAsync(pipeline)).Entity;
            await this._context.SaveChangesAsync();
            return pipeline;
        }

        public async Task<Pipeline> Read(int id)
        {
            return await this._context.Pipelines.FindAsync(id);
        }

        public async Task Update(Pipeline pipeline)
        {
            this._context.Pipelines.Update(pipeline);
            await this._context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            this._context.Pipelines.Remove(await this._context.Pipelines.FindAsync(id));
            await this._context.SaveChangesAsync();
        }

        public async Task<ICollection<Pipeline>> ReadAll()
        {
            return this._context.Pipelines.ToList();
        }
    }
}