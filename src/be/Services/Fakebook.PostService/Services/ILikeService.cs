using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Services
{
    public interface ILikeService
    {
        Task<Like> ChangeLikeStateAsync(Like model);
    }
}
