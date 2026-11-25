/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Post;
using Datagaucha.Domain.Post;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework.Post;

public class EFReactionRepository(DatagauchaDbContext dbContext) : IReactionRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public Task<Reaction?> GetReaction(long userId, long postId)
    {
        return this.dbContext.Reactions
            .FirstOrDefaultAsync(reaction =>
                reaction.User != null && reaction.User.Id == userId
                && reaction.Post != null && reaction.Post.Id == postId);
    }

    public async Task<Dictionary<long, ReactionType>> GetReactionsByUserAndPostIds(
        long userId,
        List<long> postIds)
    {
        if (postIds == null || postIds.Count == 0)
            return new Dictionary<long, ReactionType>();

        return await this.dbContext.Reactions
            .Where(reaction =>
                reaction.User != null && reaction.User.Id == userId
                && reaction.Post != null && postIds.Contains(reaction.Post.Id))
            .ToDictionaryAsync(
                reaction => reaction.Post.Id,
                reaction => reaction.Type);
    }

    // <-- NUEVO MÉTODO PARA CONTAR LIKES Y DISLIKES
    public async Task<Dictionary<long, (int Likes, int Dislikes)>> GetReactionCountsByPostIds(List<long> postIds)
    {
        if (postIds == null || postIds.Count == 0)
            return new Dictionary<long, (int, int)>();

        var counts = await this.dbContext.Reactions
            .Where(r => r.Post != null && postIds.Contains(r.Post.Id))
            .GroupBy(r => r.Post.Id)
            .Select(g => new
            {
                PostId = g.Key,
                Likes = g.Count(r => r.Type == ReactionType.Like),
                Dislikes = g.Count(r => r.Type == ReactionType.Dislike)
            })
            .ToListAsync();

        return counts.ToDictionary(c => c.PostId, c => (c.Likes, c.Dislikes));
    }

    public async Task<bool> Create(Reaction reaction)
    {
        await this.dbContext.Reactions.AddAsync(reaction);
        return true;
    }

    public Task<bool> Delete(Reaction reaction)
    {
        this.dbContext.Reactions.Remove(reaction);
        return Task.FromResult(true);
    }
}
