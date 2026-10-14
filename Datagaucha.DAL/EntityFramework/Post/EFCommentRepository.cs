/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Post;
using Datagaucha.Domain.Post;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework.Post;

public class EFCommentRepository(DatagauchaDbContext dbContext) : ICommentRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<List<Comment>> GetCommentsByPostId(long postId, int currentPage, int pageSize)
    {
        List<Comment> comments = await this.dbContext.Comments
            .Where(comment => comment.Post != null && comment.Post.Id == postId)
            .OrderByDescending(comment => comment.CreatedAt)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return comments;
    }

    public async Task<Comment?> GetCommentById(long commentId)
    {
        return await this.dbContext.Comments.FindAsync(commentId);
    }

    public async Task<bool> Create(Comment comment)
    {
        await this.dbContext.Comments.AddAsync(comment);
        return true;
    }

    public Task<bool> Delete(Comment comment)
    {
        this.dbContext.Comments.Remove(comment);
        return Task.FromResult(true);
    }
}
