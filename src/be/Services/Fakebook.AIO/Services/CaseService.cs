using Fakebook.AIO.Entity;
using Fakebook.AIO.Repositories;
using Fakebook.AIO.Services;
using Fakebook.DataAccessLayer.Interfaces;

namespace Fakebook.UserService.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CaseService(ICaseRepository caseRepository, IUnitOfWork unitOfWork)
        {
            _caseRepository = caseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Case> CreateAsync(Case cas)
        {
            cas.Id = Guid.NewGuid().ToString();
            cas.IsDeleted = false;
            cas.NumberOfSuccess = 0;
            cas.NumberOfFailed = 0;
            cas.CreatedBy = "System";
            cas.LastModifiedBy = "System";
            cas.CreatedDate = DateTime.Now;
            cas.LastModifiedDate = DateTime.Now;

            await _caseRepository.AddAsync(cas);
            await _unitOfWork.CompleteAsync();

            return cas;
        }

        public async Task DeleteAsync(string id)
        {
            var cas = await _caseRepository.FindFirstAsync(e => e.Id == id) ??
                throw new Exception("The record not found");

            cas.IsDeleted = true;
            cas.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Case>> GetAllAsync()
        {
            var cases = await _caseRepository.FindAsync(e => !e.IsDeleted);
            return cases.OrderBy(e => e.CreatedDate);
        }

        public async Task<Case?> GetByIdAsync(string id)
        {
            return await _caseRepository.FindFirstAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(string id, Case cas)
        {
            var existingCas = await _caseRepository.FindFirstAsync(e => e.Id == id) ??
                throw new Exception("The record not found");

            existingCas.Name = cas.Name;
            existingCas.Description = cas.Description;
            existingCas.JobName = cas.JobName;
            existingCas.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }
    }
}
