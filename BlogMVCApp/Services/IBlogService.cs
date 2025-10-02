using BlogMVCApp.Models;

namespace BlogMVCApp.Services
{
    public interface IBlogService
    {
        // Post operations
        Task<Post> CreatePostAsync(Post post);
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post?> GetPostBySlugAsync(string slug);
        Task<IEnumerable<Post>> GetPublishedPostsAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId, int page = 1, int pageSize = 10);
        Task<Post> UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task<string> EnsureUniqueSlugAsync(string slug, int? excludePostId = null);

        // Category operations
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryBySlugAsync(string slug);

        // Tag operations
        Task<Tag> GetOrCreateTagAsync(string tagName);
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task AssignTagsToPostAsync(int postId, IEnumerable<string> tagNames);

        // Comment operations
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);

        // Statistics
        Task<object> GetUserStatisticsAsync(string userId);
        Task IncrementPostViewCountAsync(int postId);
    }
}