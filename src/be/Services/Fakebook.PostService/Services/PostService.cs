using Fakebook.DataAccessLayer.HttpRequestHandling;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.PostService.Entity;
using Fakebook.PostService.Repositories;
using Fakebook.UserService.Dtos.Users;

namespace Fakebook.PostService.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<Post> CreateAsync(CreatePostRequest request)
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext()
                ?? throw new Exception("The authenticated user is failed");

            _ = await _userRepository.FindFirstAsync(e => e.Id == currentUser.UserId)
                ?? throw new Exception("The user id in invalid");

            var post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Content = request.Content,
                ViewMode = request.ViewMode,
                OwnerId = currentUser.UserId,
                CreatedBy = currentUser.UserId,
                LastModifiedBy = currentUser.UserId,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            await _postRepository.AddAsync(post);
            await _unitOfWork.CompleteAsync();

            return post;
        }

        public async Task UpdateAsync(string postId, UpdatePostRequest request)
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext()
                ?? throw new Exception("The authenticated user is failed");

            _ = await _userRepository.FindFirstAsync(e => e.Id == currentUser.UserId)
                ?? throw new Exception("The user id in invalid");

            var post = await _postRepository.FindFirstAsync(e => e.Id == postId && !e.IsDeleted)
                ?? throw new Exception("The post is invalid");

            if (post.OwnerId != currentUser.UserId)
            {
                throw new Exception("Update post is not allow");
            }

            post.Content = request.Content;
            post.ViewMode = request.ViewMode;
            post.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _postRepository.FindAsync(e => !e.IsDeleted);
        }

        public async Task<IEnumerable<Post>> GetByCurrentUserAsync()
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext();

            if (currentUser is null) throw new Exception("The authenticated user is failed");

            return await _postRepository.FindAsync(e => e.OwnerId == currentUser.UserId && !e.IsDeleted);
        }

        public async Task DeleteAsync(string postId)
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext()
                ?? throw new Exception("The authenticated user is failed");

            _ = await _userRepository.FindFirstAsync(e => e.Id == currentUser.UserId)
                ?? throw new Exception("The user id in invalid");

            var post = await _postRepository.FindFirstAsync(e => e.Id == postId && !e.IsDeleted)
                ?? throw new Exception("The post is invalid");

            if (post.OwnerId != currentUser.UserId)
            {
                throw new Exception("Update post is not allow");
            }

            post.IsDeleted = true;
            post.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<Post> GetByIdAsync(string postId)
        {
            return await _postRepository.GetPostById(postId)
                ?? throw new Exception("The post is invalid");
        }
    }
}
