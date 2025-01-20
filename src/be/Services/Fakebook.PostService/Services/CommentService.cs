using Fakebook.DataAccessLayer.HttpRequestHandling;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.PostService.Dtos.Comments;
using Fakebook.PostService.Entity;
using Fakebook.PostService.Repositories;
using Fakebook.UserService.Dtos.Users;

namespace Fakebook.PostService.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IPostRepository _postRepository;

        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, IPostRepository postRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<Comment> CreateComment(Comment model)
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext()
                ?? throw new Exception("The authenticated user is failed");

            _ = await _userRepository.FindFirstAsync(e => e.Id == currentUser.UserId)
               ?? throw new Exception("The user id in invalid");

            if (!string.IsNullOrWhiteSpace(model.PostId) && !string.IsNullOrWhiteSpace(model.ParentCommentId))
            {
                throw new Exception("The comment just belong to only one of post or another comment");
            }

            if (!string.IsNullOrWhiteSpace(model.PostId))
            {
                model.ParentCommentId = null;
                _ = await _postRepository.FindFirstAsync(e => e.Id == model.PostId && !e.IsDeleted)
                   ?? throw new Exception("The post is invalid");
            }

            if (!string.IsNullOrWhiteSpace(model.ParentCommentId))
            {
                model.PostId = null;
                _ = await _commentRepository.FindFirstAsync(e => e.Id == model.ParentCommentId && !e.IsDeleted)
                   ?? throw new Exception("The comment is invalid");
            }

            model.Id = Guid.NewGuid().ToString();
            model.CreatedBy = currentUser.UserId;
            model.UserId = currentUser.UserId;
            model.LastModifiedBy = currentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.LastModifiedDate = DateTime.Now;
            model.IsDeleted = false;

            await _commentRepository.AddAsync(model);
            await _unitOfWork.CompleteAsync();

            return model;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.FindAsync(e => !e.IsDeleted);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(string postId)
        {
            var _ = await _postRepository.FindFirstAsync(e => e.Id == postId && !e.IsDeleted)
                ?? throw new Exception("The post id is invalid");

            return await _commentRepository.FindAsync(e => e.PostId == postId && !e.IsDeleted);
        }
    }
}
