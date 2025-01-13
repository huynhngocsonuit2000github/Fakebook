using Fakebook.AIO.Entity;
using Fakebook.AIO.Repositories;
using Fakebook.AIO.Services;
using Fakebook.DataAccessLayer.Interfaces;

namespace Fakebook.UserService.Services
{
    public class PipelineService : IPipelineService
    {
        private readonly IPipelineRepository _pipelineRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PipelineService(IPipelineRepository pipelineRepository, IUnitOfWork unitOfWork)
        {
            _pipelineRepository = pipelineRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Pipeline> CreateAsync(Pipeline pipeline)
        {
            pipeline.Id = Guid.NewGuid().ToString();
            pipeline.IsDeleted = false;
            pipeline.CreatedBy = "System";
            pipeline.LastModifiedBy = "System";
            pipeline.CreatedDate = DateTime.Now;
            pipeline.LastModifiedDate = DateTime.Now;

            await _pipelineRepository.AddAsync(pipeline);
            await _unitOfWork.CompleteAsync();

            return pipeline;
        }

        public async Task DeleteAsync(string id)
        {
            var pipeline = await _pipelineRepository.FindFirstAsync(e => e.Id == id) ??
                throw new Exception("The record not found");

            pipeline.IsDeleted = true;
            pipeline.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Pipeline>> GetAllAsync()
        {
            var Pipelines = await _pipelineRepository.FindAsync(e => !e.IsDeleted);
            return Pipelines.OrderBy(e => e.CreatedDate);
        }

        public async Task<Pipeline?> GetByIdAsync(string id)
        {
            return await _pipelineRepository.FindFirstAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(string id, Pipeline pipeline)
        {
            var existingPipeline = await _pipelineRepository.FindFirstAsync(e => e.Id == id) ??
                throw new Exception("The record not found");

            existingPipeline.JobName = pipeline.JobName;
            existingPipeline.JobDescription = pipeline.JobDescription;
            existingPipeline.AuthToken = pipeline.AuthToken;
            existingPipeline.PipelineContent = pipeline.PipelineContent;
            existingPipeline.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }
    }
}
