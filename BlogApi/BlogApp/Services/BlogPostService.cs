using BlogApp.DTOs;
using BlogApp.Interfaces;
using BlogDomain.Entities;
using BlogInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogApp.Services;

public class BlogPostService : IBlogPostService
{
    private readonly BlogDbContext _context;

    public BlogPostService(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<PostResponseDto> CreatePostAsync(CreatePostDto dto)
    {
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

        return MapToDto(post);
    }

    public async Task<PostResponseDto?> GetPostByIdAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        return post == null ? null : MapToDto(post);
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync(string? term)
    {
        var query = _context.BlogPosts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
        {
            term = term.ToLower();
            query = query.Where(p =>
                p.Title.ToLower().Contains(term) ||
                p.Content.ToLower().Contains(term) ||
                p.Category.ToLower().Contains(term)
            );
        }

        var posts = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return posts.Select(MapToDto);
    }

    public async Task<PostResponseDto?> UpdatePostAsync(int id, UpdatePostDto dto)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null) return null;

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.Category = dto.Category;
        post.Tags = dto.Tags;
        post.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDto(post);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null) return false;

        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }

    private static PostResponseDto MapToDto(BlogPost post) =>
        new()
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
