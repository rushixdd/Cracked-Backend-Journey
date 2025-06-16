using BlogApp.DTOs;
using BlogApp.Interfaces;
using BlogDomain.Entities;
using BlogInfrastructure.Data;
using BlogInfrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly BlogDbContext _context;
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostService(BlogDbContext context, ILogger<BlogPostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PostResponseDto> CreatePostAsync(CreatePostDto dto)
        {
            _logger.LogInformation(LogMessages.CreatingPost);

            var post = new BlogPost
            {
                Title = dto.Title,
                Content = dto.Content,
                Category = dto.Category,
                Tags = dto.Tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            _logger.LogInformation(LogMessages.PostCreated, post.Id);

            return MapToDto(post);
        }

        public async Task<PostResponseDto?> GetPostByIdAsync(int id)
        {
            _logger.LogInformation(LogMessages.GettingPostById, id);

            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                _logger.LogWarning(LogMessages.PostNotFound, id);
                return null;
            }

            return MapToDto(post);
        }

        public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync(string? searchTerm)
        {
            _logger.LogInformation(LogMessages.GettingAllPosts);

            var query = _context.BlogPosts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowerTerm = searchTerm.ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(lowerTerm) ||
                    p.Content.ToLower().Contains(lowerTerm) ||
                    p.Category.ToLower().Contains(lowerTerm));
            }

            var posts = await query.ToListAsync();
            return posts.Select(MapToDto);
        }

        public async Task<PostResponseDto?> UpdatePostAsync(int id, UpdatePostDto dto)
        {
            _logger.LogInformation(LogMessages.UpdatingPost, id);

            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                _logger.LogWarning(LogMessages.PostNotFound, id);
                return null;
            }

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.Category = dto.Category;
            post.Tags = dto.Tags;
            post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(LogMessages.PostUpdated, id);

            return MapToDto(post);
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            _logger.LogInformation(LogMessages.DeletingPost, id);

            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                _logger.LogWarning(LogMessages.PostNotFound, id);
                return false;
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();

            _logger.LogInformation(LogMessages.PostDeleted, id);
            return true;
        }

        private PostResponseDto MapToDto(BlogPost post)
        {
            return new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Category = post.Category,
                Tags = post.Tags,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
        }
    }
}
