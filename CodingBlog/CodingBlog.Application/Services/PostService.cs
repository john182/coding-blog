using CodingBlog.Domain.Entities;
using CodingBlog.Domain.Repositories;
using CodingBlog.Infrastructure.Notification;
using FluentResults;

namespace CodingBlog.Application.Services;

public interface IPostService
{
    Task<IReadOnlyList<Post>> GetAll(CancellationToken cancellationToken);

    Task<Result<Post>> GetById(int id, CancellationToken cancellationToken);

    Task<Result<Post>> Create(Post post, CancellationToken cancellationToken);
    Task<Result<Post>> Update(int id, Post post, CancellationToken cancellationToken);
    Task<Result> Delete(int id, CancellationToken cancellationToken);
}

public class PostService : IPostService
{
    private readonly IPostNotificationService _postNotificationService;
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository, IPostNotificationService postNotificationService)
    {
        _postRepository = postRepository;
        _postNotificationService = postNotificationService;
    }

    public async Task<IReadOnlyList<Post>> GetAll(CancellationToken cancellationToken)
        => await _postRepository.GetAll(cancellationToken);

    public async Task<Result<Post>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _postRepository.GetById(id, cancellationToken);

        return result is null
            ? Result.Fail("Not Update Post")
            : Result.Ok(result);
    }

    public async Task<Result<Post>> Create(Post post, CancellationToken cancellationToken)
    {
        var createdPost = await _postRepository.Create(post, cancellationToken);
        await _postNotificationService.SendPostNotificationAsync($"New post: {createdPost.Title}");
        return Result.Ok(createdPost);
    }

    public async Task<Result<Post>> Update(int id, Post post, CancellationToken cancellationToken)
    {
        var result = await _postRepository.Update(id, post, cancellationToken);

        return result is null
            ? Result.Fail("Not Update Post")
            : Result.Ok(result);
    }

    public async Task<Result> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _postRepository.Delete(id, cancellationToken);

        return result ? Result.Ok() : Result.Fail("Note Delete Post");
    }
}