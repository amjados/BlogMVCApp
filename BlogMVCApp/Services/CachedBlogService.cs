using BlogMVCApp.Models;

namespace BlogMVCApp.Services
{
    public class CachedBlogService : IBlogService
    {
        private readonly IBlogService _blogService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CachedBlogService> _logger;

        // Cache key constants
        private const string POST_BY_ID_KEY = "post:id:{0}";
        private const string POST_BY_SLUG_KEY = "post:slug:{0}";
        private const string PUBLISHED_POSTS_KEY = "posts:published:{0}:{1}";
        private const string POSTS_BY_AUTHOR_KEY = "posts:author:{0}:{1}:{2}";
        private const string CATEGORIES_ACTIVE_KEY = "categories:active";
        private const string CATEGORY_BY_ID_KEY = "category:id:{0}";
        private const string CATEGORY_BY_SLUG_KEY = "category:slug:{0}";
        private const string TAGS_KEY = "tags:all";
        private const string COMMENTS_BY_POST_KEY = "comments:post:{0}";
        private const string USER_STATS_KEY = "stats:user:{0}";

        // Cache expiration times
        private static readonly TimeSpan POST_CACHE_DURATION = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan CATEGORY_CACHE_DURATION = TimeSpan.FromHours(1);
        private static readonly TimeSpan TAG_CACHE_DURATION = TimeSpan.FromHours(2);
        private static readonly TimeSpan COMMENT_CACHE_DURATION = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan STATS_CACHE_DURATION = TimeSpan.FromMinutes(10);

        public CachedBlogService(IBlogService blogService, ICacheService cacheService, ILogger<CachedBlogService> logger)
        {
            _blogService = blogService;
            _cacheService = cacheService;
            _logger = logger;
        }

        // Post operations
        public async Task<Post> CreatePostAsync(Post post)
        {
            var result = await _blogService.CreatePostAsync(post);

            // Invalidate related caches
            await InvalidatePostCaches(post.AuthorId);

            return result;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var cacheKey = string.Format(POST_BY_ID_KEY, id);
            var cachedPost = await _cacheService.GetAsync<Post>(cacheKey);

            if (cachedPost != null)
            {
                return cachedPost;
            }

            var post = await _blogService.GetPostByIdAsync(id);
            if (post != null)
            {
                await _cacheService.SetAsync(cacheKey, post, POST_CACHE_DURATION);
                // Also cache by slug
                var slugCacheKey = string.Format(POST_BY_SLUG_KEY, post.Slug);
                await _cacheService.SetAsync(slugCacheKey, post, POST_CACHE_DURATION);
            }

            return post;
        }

        public async Task<Post?> GetPostBySlugAsync(string slug)
        {
            var cacheKey = string.Format(POST_BY_SLUG_KEY, slug);
            var cachedPost = await _cacheService.GetAsync<Post>(cacheKey);

            if (cachedPost != null)
            {
                return cachedPost;
            }

            var post = await _blogService.GetPostBySlugAsync(slug);
            if (post != null)
            {
                await _cacheService.SetAsync(cacheKey, post, POST_CACHE_DURATION);
                // Also cache by ID
                var idCacheKey = string.Format(POST_BY_ID_KEY, post.Id);
                await _cacheService.SetAsync(idCacheKey, post, POST_CACHE_DURATION);
            }

            return post;
        }

        public async Task<IEnumerable<Post>> GetPublishedPostsAsync(int page = 1, int pageSize = 10)
        {
            var cacheKey = string.Format(PUBLISHED_POSTS_KEY, page, pageSize);
            var cachedPosts = await _cacheService.GetAsync<List<Post>>(cacheKey);

            if (cachedPosts != null)
            {
                return cachedPosts;
            }

            var posts = (await _blogService.GetPublishedPostsAsync(page, pageSize)).ToList();
            await _cacheService.SetAsync(cacheKey, posts, POST_CACHE_DURATION);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId, int page = 1, int pageSize = 10)
        {
            var cacheKey = string.Format(POSTS_BY_AUTHOR_KEY, authorId, page, pageSize);
            var cachedPosts = await _cacheService.GetAsync<List<Post>>(cacheKey);

            if (cachedPosts != null)
            {
                return cachedPosts;
            }

            var posts = (await _blogService.GetPostsByAuthorAsync(authorId, page, pageSize)).ToList();
            await _cacheService.SetAsync(cacheKey, posts, POST_CACHE_DURATION);

            return posts;
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            var result = await _blogService.UpdatePostAsync(post);

            // Invalidate caches for this specific post
            await _cacheService.RemoveAsync(string.Format(POST_BY_ID_KEY, post.Id));
            await _cacheService.RemoveAsync(string.Format(POST_BY_SLUG_KEY, post.Slug));

            // Invalidate related caches
            await InvalidatePostCaches(post.AuthorId);

            return result;
        }

        public async Task DeletePostAsync(int id)
        {
            // Get post first to know what to invalidate
            var post = await GetPostByIdAsync(id);

            await _blogService.DeletePostAsync(id);

            if (post != null)
            {
                await _cacheService.RemoveAsync(string.Format(POST_BY_ID_KEY, id));
                await _cacheService.RemoveAsync(string.Format(POST_BY_SLUG_KEY, post.Slug));
                await InvalidatePostCaches(post.AuthorId);
            }
        }

        public async Task<string> EnsureUniqueSlugAsync(string slug, int? excludePostId = null)
        {
            // This doesn't need caching as it's used during creation/update
            return await _blogService.EnsureUniqueSlugAsync(slug, excludePostId);
        }

        // Category operations
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var result = await _blogService.CreateCategoryAsync(category);

            // Invalidate category caches
            await _cacheService.RemoveAsync(CATEGORIES_ACTIVE_KEY);

            return result;
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            var cachedCategories = await _cacheService.GetAsync<List<Category>>(CATEGORIES_ACTIVE_KEY);

            if (cachedCategories != null)
            {
                return cachedCategories;
            }

            var categories = (await _blogService.GetActiveCategoriesAsync()).ToList();
            await _cacheService.SetAsync(CATEGORIES_ACTIVE_KEY, categories, CATEGORY_CACHE_DURATION);

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var cacheKey = string.Format(CATEGORY_BY_ID_KEY, id);
            var cachedCategory = await _cacheService.GetAsync<Category>(cacheKey);

            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            var category = await _blogService.GetCategoryByIdAsync(id);
            if (category != null)
            {
                await _cacheService.SetAsync(cacheKey, category, CATEGORY_CACHE_DURATION);
            }

            return category;
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            var cacheKey = string.Format(CATEGORY_BY_SLUG_KEY, slug);
            var cachedCategory = await _cacheService.GetAsync<Category>(cacheKey);

            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            var category = await _blogService.GetCategoryBySlugAsync(slug);
            if (category != null)
            {
                await _cacheService.SetAsync(cacheKey, category, CATEGORY_CACHE_DURATION);
            }

            return category;
        }

        // Tag operations
        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            // Don't cache tag creation as it might create new tags
            var result = await _blogService.GetOrCreateTagAsync(tagName);

            // Invalidate tag list cache
            await _cacheService.RemoveAsync(TAGS_KEY);

            return result;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            var cachedTags = await _cacheService.GetAsync<List<Tag>>(TAGS_KEY);

            if (cachedTags != null)
            {
                return cachedTags;
            }

            var tags = (await _blogService.GetTagsAsync()).ToList();
            await _cacheService.SetAsync(TAGS_KEY, tags, TAG_CACHE_DURATION);

            return tags;
        }

        public async Task AssignTagsToPostAsync(int postId, IEnumerable<string> tagNames)
        {
            await _blogService.AssignTagsToPostAsync(postId, tagNames);

            // Invalidate tag and post caches
            await _cacheService.RemoveAsync(TAGS_KEY);
            await _cacheService.RemoveAsync(string.Format(POST_BY_ID_KEY, postId));
        }

        // Comment operations
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            var result = await _blogService.CreateCommentAsync(comment);

            // Invalidate comment cache for the post
            await _cacheService.RemoveAsync(string.Format(COMMENTS_BY_POST_KEY, comment.PostId));

            // Invalidate post cache to update comment count
            await _cacheService.RemoveAsync(string.Format(POST_BY_ID_KEY, comment.PostId));

            return result;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId)
        {
            var cacheKey = string.Format(COMMENTS_BY_POST_KEY, postId);
            var cachedComments = await _cacheService.GetAsync<List<Comment>>(cacheKey);

            if (cachedComments != null)
            {
                return cachedComments;
            }

            var comments = (await _blogService.GetCommentsByPostAsync(postId)).ToList();
            await _cacheService.SetAsync(cacheKey, comments, COMMENT_CACHE_DURATION);

            return comments;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            var result = await _blogService.UpdateCommentAsync(comment);

            // Invalidate comment cache
            await _cacheService.RemoveAsync(string.Format(COMMENTS_BY_POST_KEY, comment.PostId));

            return result;
        }

        public async Task DeleteCommentAsync(int id)
        {
            // We need to get the comment first to know which post's cache to invalidate
            // This is not ideal, but needed for proper cache invalidation
            await _blogService.DeleteCommentAsync(id);

            // For simplicity, we'll invalidate all comment caches
            // In a production system, you might want to track comment-post relationships
            await _cacheService.RemovePatternAsync("comments:post:");
        }

        // Statistics
        public async Task<object> GetUserStatisticsAsync(string userId)
        {
            var cacheKey = string.Format(USER_STATS_KEY, userId);
            var cachedStats = await _cacheService.GetAsync<object>(cacheKey);

            if (cachedStats != null)
            {
                return cachedStats;
            }

            var stats = await _blogService.GetUserStatisticsAsync(userId);
            await _cacheService.SetAsync(cacheKey, stats, STATS_CACHE_DURATION);

            return stats;
        }

        public async Task IncrementPostViewCountAsync(int postId)
        {
            await _blogService.IncrementPostViewCountAsync(postId);

            // Invalidate the post cache since view count changed
            await _cacheService.RemoveAsync(string.Format(POST_BY_ID_KEY, postId));

            // Note: We don't invalidate slug cache here for performance reasons
            // View count updates are frequent and we can accept slight inconsistency
        }

        private async Task InvalidatePostCaches(string authorId)
        {
            // Invalidate published posts cache
            await _cacheService.RemovePatternAsync("posts:published:");

            // Invalidate author's posts cache
            await _cacheService.RemovePatternAsync($"posts:author:{authorId}:");

            // Invalidate user statistics
            await _cacheService.RemoveAsync(string.Format(USER_STATS_KEY, authorId));
        }
    }
}