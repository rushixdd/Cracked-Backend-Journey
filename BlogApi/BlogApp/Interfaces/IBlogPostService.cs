using BlogApp.DTOs;

namespace BlogApp.Interfaces;

public interface IBlogPostService
{
    Task<PostResponseDto> CreatePostAsync(CreatePostDto dto);
    Task<PostResponseDto?> GetPostByIdAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetAllPostsAsync(string? searchTerm);
    Task<PostResponseDto?> UpdatePostAsync(int id, UpdatePostDto dto);
    Task<bool> DeletePostAsync(int id);
}
