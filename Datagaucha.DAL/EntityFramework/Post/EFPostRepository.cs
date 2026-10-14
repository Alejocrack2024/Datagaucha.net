/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.DAL.EntityFramework.Post;

using Datagaucha.DAL.interfaces.Post;
using Datagaucha.Domain.Auth;
using Microsoft.EntityFrameworkCore;

public class EFPostRepository(DatagauchaDbContext dbContext) : IPostRepository
{
    private const string OrderByCreatedAt = "CREATEDAT";
    private const string OrderByBody = "BODY";
    private const string Ascending = "ASC";

    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<List<Domain.Post.Post>> GetPosts(
        User user,
        int currentPage,
        int pageSize,
        string orderBy,
        string orderDirection,
        string? search)
    {
        IQueryable<long> followedUserIds = dbContext.Follows
            .Where(follow => follow.Follower != null && follow.Follower.Id == user.Id)
            .Select(follow => follow.Following!.Id);

        IQueryable<Datagaucha.Domain.Post.Post> posts = dbContext.Posts
            .Where(post => post.User != null
                && (post.User.Id == user.Id || followedUserIds.Contains(post.User.Id)));

        if (!String.IsNullOrEmpty(search))
        {
            posts = posts.Where(post => post.Body.Contains(search));
        }

        IQueryable<Datagaucha.Domain.Post.Post> orderedPosts = AddOrder(posts, orderBy, orderDirection);

        List<Datagaucha.Domain.Post.Post> result = await orderedPosts
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return result;
    }

    public async Task<Domain.Post.Post?> GetPostById(long postId)
    {
        return await this.dbContext.Posts.FindAsync(postId);
    }

    public async Task<bool> Create(Domain.Post.Post post)
    {
        await this.dbContext.Posts.AddAsync(post);
        return true;
    }

    public Task<bool> Delete(Domain.Post.Post post)
    {
        this.dbContext.Posts.Remove(post);
        return Task.FromResult(true);
    }

    private static IQueryable<Domain.Post.Post> AddOrder(
        IQueryable<Domain.Post.Post> posts,
        string orderBy,
        string orderDirection)
    {
        string order = String.IsNullOrEmpty(orderBy) ? OrderByCreatedAt : orderBy.ToUpper();
        string direction = String.IsNullOrEmpty(orderDirection) ? "DESC" : orderDirection.ToUpper();

        IQueryable<Domain.Post.Post> orderedPosts;

        if (order.Equals(OrderByBody))
        {
            orderedPosts = direction.Equals(Ascending)
                ? posts.OrderBy(post => post.Body)
                : posts.OrderByDescending(post => post.Body);
        }
        else
        {
            orderedPosts = direction.Equals(Ascending)
                ? posts.OrderBy(post => post.Id)
                : posts.OrderByDescending(post => post.Id);
        }

        return orderedPosts;
    }
}
