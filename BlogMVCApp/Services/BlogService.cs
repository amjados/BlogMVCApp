using BlogMVCApp.Data;
using BlogMVCApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BlogService> _logger;

        public BlogService(ApplicationDbContext context, ILogger<BlogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Post operations
        public async Task<Post> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> GetPostBySlugAsync(string slug)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.Comments.Where(c => c.IsApproved))
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<IEnumerable<Post>> GetPublishedPostsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Where(p => p.Status == "Published")
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId, int page = 1, int pageSize = 10)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            post.UpdatedAt = DateTime.UtcNow;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> EnsureUniqueSlugAsync(string slug, int? excludePostId = null)
        {
            var originalSlug = slug;
            var counter = 1;

            while (true)
            {
                var query = _context.Posts.Where(p => p.Slug == slug);
                if (excludePostId.HasValue)
                {
                    query = query.Where(p => p.Id != excludePostId.Value);
                }

                if (!await query.AnyAsync())
                    break;

                slug = $"{originalSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        // Category operations
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }

        // Tag operations
        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            var slug = GenerateSlug(tagName);
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Slug == slug);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName.Trim(),
                    Slug = slug,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _context.Tags.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task AssignTagsToPostAsync(int postId, IEnumerable<string> tagNames)
        {
            // Remove existing tags
            var existingPostTags = await _context.PostTags.Where(pt => pt.PostId == postId).ToListAsync();
            _context.PostTags.RemoveRange(existingPostTags);

            // Add new tags
            foreach (var tagName in tagNames.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                var tag = await GetOrCreateTagAsync(tagName.Trim());
                _context.PostTags.Add(new PostTag { PostId = postId, TagId = tag.Id });
            }

            await _context.SaveChangesAsync();
        }

        // Comment operations
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .Where(c => c.PostId == postId && c.IsApproved && c.ParentCommentId == null)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            comment.UpdatedAt = DateTime.UtcNow;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        // Statistics
        public async Task<object> GetUserStatisticsAsync(string userId)
        {
            var totalPosts = await _context.Posts.CountAsync(p => p.AuthorId == userId);
            var publishedPosts = await _context.Posts.CountAsync(p => p.AuthorId == userId && p.Status == "Published");
            var draftPosts = await _context.Posts.CountAsync(p => p.AuthorId == userId && p.Status == "Draft");
            var totalViews = await _context.Posts.Where(p => p.AuthorId == userId).SumAsync(p => p.ViewCount);
            var totalComments = await _context.Comments
                .Where(c => c.Post != null && c.Post.AuthorId == userId)
                .CountAsync();

            var recentPosts = await _context.Posts
                .Include(p => p.Category)
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Status,
                    p.CreatedAt,
                    p.ViewCount,
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized"
                })
                .ToListAsync();

            return new
            {
                TotalPosts = totalPosts,
                PublishedPosts = publishedPosts,
                DraftPosts = draftPosts,
                TotalViews = totalViews,
                TotalComments = totalComments,
                RecentPosts = recentPosts
            };
        }

        public async Task IncrementPostViewCountAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                post.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateSlug(string title)
        {
            return title.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("?", "")
                .Replace("!", "")
                .Replace("&", "and")
                .Replace("@", "at")
                .Replace("$", "")
                .Replace("%", "")
                .Replace("^", "")
                .Replace("*", "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
}