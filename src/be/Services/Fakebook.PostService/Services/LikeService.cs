using Fakebook.DataAccessLayer.HttpRequestHandling;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.PostService.Entity;
using Fakebook.PostService.Repositories;

namespace Fakebook.PostService.Services
{
    public class LikeService : ILikeService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;

        public LikeService(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, IUserRepository userRepository, IPostRepository postRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _likeRepository = likeRepository;
        }

        public async Task<Like> ChangeLikeStateAsync(Like model)
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext()
                ?? throw new Exception("The authenticated user is failed");

            _ = await _userRepository.FindFirstAsync(e => e.Id == currentUser.UserId)
               ?? throw new Exception("The user id in invalid");

            if (!string.IsNullOrWhiteSpace(model.PostId) && !string.IsNullOrWhiteSpace(model.CommentId))
            {
                throw new Exception("The like just belong to only one of post or comment");
            }

            Like? existingLike = null;

            if (!string.IsNullOrWhiteSpace(model.PostId))
            {
                model.CommentId = null;
                _ = await _postRepository.FindFirstAsync(e => e.Id == model.PostId && !e.IsDeleted)
                   ?? throw new Exception("The post is invalid");

                existingLike = await _likeRepository.FindFirstAsync(e => e.UserId == currentUser.UserId && e.PostId == model.PostId);
            }

            if (!string.IsNullOrWhiteSpace(model.CommentId))
            {
                model.PostId = null;
                _ = await _commentRepository.FindFirstAsync(e => e.Id == model.CommentId && !e.IsDeleted)
                   ?? throw new Exception("The comment is invalid");

                existingLike = await _likeRepository.FindFirstAsync(e => e.UserId == currentUser.UserId && e.CommentId == model.CommentId);
            }

            if (existingLike is null)
            {
                model.Id = Guid.NewGuid().ToString();
                model.CreatedBy = currentUser.UserId;
                model.UserId = currentUser.UserId;
                model.LastModifiedBy = currentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.LastModifiedDate = DateTime.Now;
                model.IsDeleted = false;

                await _likeRepository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return model;
            }
            else
            {
                existingLike.LastModifiedBy = currentUser.UserId;
                existingLike.LastModifiedDate = DateTime.Now;
                existingLike.IsDeleted = !existingLike.IsDeleted;

                await _unitOfWork.CompleteAsync();

                return existingLike;
            }
        }
    }
}
